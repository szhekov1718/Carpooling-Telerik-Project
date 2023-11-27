using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Carpooling.Data;
using Carpooling.Data.Enums;
using Carpooling.Data.Models;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Enums;
using Carpooling.Services.Exceptions;
using Carpooling.Services.Exceptions.Exeception.User;
using DeepAI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Carpooling.Services
{
    public class UserService : IUserService

    {
        private readonly CarpoolingContext carpoolingContext;

        public UserService(CarpoolingContext carpoolingContext)
        {
            this.carpoolingContext = carpoolingContext;
        }

        public UserDTO GetBy(string value, string prop)
        {
            UserDTO user;
            switch(prop)
            {
                case "Username":
                user = this.carpoolingContext.Users.AsEnumerable()
                                               .Where(u => u.IsDeleted == false && u.Username == value)
                                               .Select(u => new UserDTO(u))
                                               .FirstOrDefault();
                break;

                case "Email":
                user = this.carpoolingContext.Users.AsEnumerable()
                                               .Where(u => u.IsDeleted == false && u.Email == value)
                                               .Select(u => new UserDTO(u))
                                               .FirstOrDefault();
                break;

                default:
                user = this.carpoolingContext.Users.AsEnumerable()
                                               .Where(u => u.IsDeleted == false && u.PhoneNumber == value)
                                               .Select(u => new UserDTO(u))
                                               .FirstOrDefault();
                break;
            }

            return user;
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            var user = await this.carpoolingContext.Users.Include(u => u.Trips)
                                               .Where(u => u.IsDeleted == false && u.Username == username)
                                               .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await this.carpoolingContext.Users.Include(u => u.Trips)
                                               .Where(u => u.IsDeleted == false && u.Id == id)
                                               .FirstOrDefaultAsync();

            return user;
        }

        public async Task<ICollection<UserDTO>> GetAllUsersAsync()
        {
            var result = await this.carpoolingContext.Users
                .Where(u => u.IsDeleted == false)
                .Select(u => new UserDTO(u))
                .ToListAsync();

            var userCollection = await this.carpoolingContext.Users
                .ToListAsync();

            if(!result.Any())
            {
                throw new EntityNotFound(ExceptionMessages.EntityNotFound);
            }

            return result;
        }

        private void NudityCheck(string imageUrl)
        {
            DeepAI_API api = new DeepAI_API(apiKey: "bfc080e2-9e62-4894-a291-48a2485e919c");

            StandardApiResponse response = api.callStandardApi("nsfw-detector", new
            {
                image = imageUrl,
            });

            var jsonString = api.objectAsJsonString(response);
            var jsonObj = (JObject)JsonConvert.DeserializeObject(jsonString);

            var nsfwScore = jsonObj["output"]["nsfw_score"].Value<int>();

            if(nsfwScore > 0.5)
            {
                throw new NudeAvatarException(ExceptionMessages.NoNudeAvatars);
            }
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            if(this.GetBy(userDTO.Username, FilterBy.Username.ToString()) != null)
            {
                throw new UserNameExistException(ExceptionMessages.UsernameExists);
            }

            if(!ValidateEmail(userDTO.Email))
            {
                throw new EmailIncorrectException(ExceptionMessages.EmailIncorrect);
            }

            if(this.GetBy(userDTO.Email, FilterBy.Email.ToString()) != null)
            {
                throw new EmailExistsException(ExceptionMessages.EmailExists);
            }

            if(userDTO.Username.Length < 2 || userDTO.Username.Length > 20)
            {
                throw new ArgumentException(ExceptionMessages.InvalidUsernameLength);
            }

            if(userDTO.FirstName.Length < 2 || userDTO.FirstName.Length > 20)
            {
                throw new ArgumentException(ExceptionMessages.InvalidFirstNameLength);
            }

            if(userDTO.LastName.Length < 2 || userDTO.LastName.Length > 20)
            {
                throw new ArgumentException(ExceptionMessages.InvalidLastNameLength);
            }

            Regex rgx = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
            if(userDTO.Password == null)
            {
                throw new ArgumentException(ExceptionMessages.PasswordRequired);
            }

            if(!rgx.IsMatch(userDTO.Password))
            {
                throw new ArgumentException(ExceptionMessages.PasswordSpecialCharacters);
            }

            if(userDTO.ImageUrl != null)
            {
                this.NudityCheck(userDTO.ImageUrl);
            }
            else
                userDTO.ImageUrl = "https://iupac.org/wp-content/uploads/2018/05/default-avatar.png";


            var hashedPassword = HashPassword(userDTO.Password);

            User newUser = new User
            {
                Username = userDTO.Username,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Password = hashedPassword,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email,
                Image = userDTO.ImageUrl
            };

            await this.carpoolingContext.Users.AddAsync(newUser);

            newUser.UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        UserId = newUser.Id,
                        RoleId = Guid.Parse("943b692d-330e-405d-a019-c3d728442144")
                    }
                };

            await this.carpoolingContext.SaveChangesAsync();
            UserDTO result = new UserDTO(newUser);

            this.SendEmail(newUser.Username, newUser.Email);

            return result;
        }

        private void SendEmail(string username, string email)
        {
            using(MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("carpoolingtelerik@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Welcome to Carpooling!";
                mail.IsBodyHtml = true;
                mail.Body = $"<h1> Hello {username},</h1> " + "\n\n" +
                    $" {Environment.NewLine} We are happy to have you as a customer. We strive to provide the best options for carpooling and carsharing! " + "\n\n" +
                    $"{Environment.NewLine} We hope you can find everything you need in our website! " + "\n\n" +
                    $"{Environment.NewLine} Happy traveling! ";


                using(SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("carpoolingtelerik@gmail.com", "qhtenlak1");
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mail);
                }
            }
        }

        public string HashPassword(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            SHA256Managed hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(bytes);

            string hashString = string.Empty;

            foreach(byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }

            return hashString;
        }

        public async Task<UserRatingDTO> UserRatingLikePassengerOrDriver(Guid userId, string value)
        {
            var user = await this.carpoolingContext.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            if(user == null)
            {
                throw new UserNameExistException(ExceptionMessages.InvalidUser);
            }

            var roleId = await this.carpoolingContext.Roles.Where(r => r.TravelRole.ToString() == value)
                                                           .Select(r => r.Id)
                                                           .FirstOrDefaultAsync();

            var ratingResult = await this.carpoolingContext.Feedbacks.Where(f => f.UserId == userId && f.RoleId == roleId)
                                                                     .Select(f => f.Rating)
                                                                     .ToListAsync();
            double rating = 0;
            if(ratingResult.Any())
            {
                rating = ratingResult.Average();
            }

            var comments = await this.carpoolingContext.Feedbacks.Where(f => f.UserId == userId && f.RoleId == roleId)
                                                                 .Select(f => f.Description)
                                                                 .ToListAsync();

            var userRating = new UserRatingDTO(rating, user.Username, comments);
            await this.carpoolingContext.SaveChangesAsync();

            return userRating;
        }

        public async Task<UserDTO> UpdateUserAsync(string username, UserDTO userDTO)
        {
            var user = await this.GetUserByUserNameAsync(username);
            if(user == null)
            {
                throw new UserNotExistException(ExceptionMessages.InvalidUser);
            }

            if(userDTO == null)
            {
                throw new UserNotExistException(ExceptionMessages.InvalidUser);
            }

            user.FirstName = userDTO.FirstName ?? user.FirstName;
            user.LastName = userDTO.LastName ?? user.LastName;

            if(userDTO.Username != null)
            {
                var checkUsername = this.GetBy(userDTO.Username, FilterBy.Username.ToString());

                if(checkUsername != null && checkUsername.Username != user.Username)
                {
                    throw new ArgumentException(ExceptionMessages.UsernameExists);
                }

                user.Username = userDTO.Username;
            }

            if(userDTO.Email != null)
            {

                if(!ValidateEmail(userDTO.Email))
                {
                    throw new EmailIncorrectException(ExceptionMessages.EmailIncorrect);
                }

                var checkUserEmail = this.GetBy(userDTO.Email, FilterBy.Email.ToString());

                if(checkUserEmail != null && checkUserEmail.Email != user.Email)
                {
                    throw new ArgumentException(ExceptionMessages.EmailExists);
                }

                user.Email = userDTO.Email;
            }

            if(userDTO.PhoneNumber != null)
            {
                user.PhoneNumber = userDTO.PhoneNumber;
            }
            if(userDTO.ImageUrl != null)
            {
                this.NudityCheck(userDTO.ImageUrl);
                user.Image = userDTO.ImageUrl;
            }

            await this.carpoolingContext.SaveChangesAsync();

            UserDTO result = new UserDTO(user);

            return result;
        }

        public bool ValidateEmail(string email)
        {
            var foo = new EmailAddressAttribute();
            bool bar = false;

            if(new EmailAddressAttribute().IsValid(email))
            {
                bar = true;
            }

            return bar;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {

            var userForDelete = await this.GetUserByUserNameAsync(username) ??
                throw new UserNotExistException(ExceptionMessages.InvalidUser);

            userForDelete.IsDeleted = true;
            await this.carpoolingContext.SaveChangesAsync();

            return userForDelete.IsDeleted;
        }

        public async Task<bool> IsUserBlocked(string username)
        {
            var user = await this.GetUserByUserNameAsync(username);
            if(user == null)
            {
                throw new UserNotExistException(ExceptionMessages.InvalidUser);
            }

            return user.IsBlocked;
        }

        public async Task<User> BlockUnblockUserAsync(string username, string action)
        {
            var user = this.GetUserByUserNameAsync(username);

            if(user.Result == null)
            {
                throw new UserNotExistException(ExceptionMessages.InvalidUser);
            }

            if(action.Equals("Block") && user.Result.IsBlocked == false)
            {
                user.Result.IsBlocked = true;
            }
            else if(action.Equals("Unblock") && user.Result.IsBlocked == true)
            {
                user.Result.IsBlocked = false;
            }

            await this.carpoolingContext.SaveChangesAsync();

            return user.Result;
        }

        public async Task<bool> MakeUserAdmin(string username)
        {
            var user = await this.carpoolingContext.Users.Where(u => u.Username == username).FirstOrDefaultAsync() ??
                throw new UserNotExistException(ExceptionMessages.InvalidUser);

            user.IsAdmin = true;
            await this.carpoolingContext.SaveChangesAsync();

            return user.IsAdmin;
        }

        public async Task<TravelStatus> DriverMarkTripCompleted(Guid driverId, Guid tripId)
        {
            var trip = await this.carpoolingContext.Trips
                        .FirstOrDefaultAsync(d => d.DriverId == driverId && d.Id == tripId) ?? throw new EntityNotFound(ExceptionMessages.EntityNotFound);

            trip.TravelStatus = TravelStatus.Completed;
            await this.carpoolingContext.SaveChangesAsync();

            return trip.TravelStatus;
        }
    }
}
