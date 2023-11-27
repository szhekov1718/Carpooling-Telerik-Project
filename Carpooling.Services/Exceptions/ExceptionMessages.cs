namespace Carpooling.Services.Exceptions
{
    public class ExceptionMessages
    {
        public const string InvalidTrip = "There is no such trip.";
        public const string InvalidTrips = "There are no trips.";

        public const string InvalidRoleForTrip = "There is no such role.";
        public const string UserHasNoRole = "User has no role.";
        public const string InvalidDateFormat = "Date format should be YYYY-MM-DD!";

        public const string InvalidUser = "There is no such user.";
        public const string CredentialsRequired = "You need to fill in your credentials!";
        public const string InvalidCredentials = "Invalid credentials!";
        public const string InvalidUserKey = "Invalid user key!";
        public const string LoginRequired = "Please Log In first!";
        public const string InvalidRegisterData = "You need to enter the necessary data!";
        public const string NoAuthority = "User has no authority for this action!";
        public const string InvalidEmail = "There is no such email.";
        public const string InvalidPassword = "Password is incorrect.";
        public const string InvalidTripComment = "Comment is required!";

        public const string InvalidFeedback = "There is no such feedback.";
        public const string InvalidCandidate = "There is no such candidate.";
        public const string NoTripCandidacy = "There is no trip candidacy yet.";
        public const string TripCandidacyExist = "Trip candidacy already exist!";

        public const string NoApprovedCandidates = "There are no approved candidates for this trip.";
        public const string NoRejectedCandidates = "There are no rejected candidates for this trip.";
        public const string RejectCandidateInDepartureDay = "Cannot reject in the day of departure.";
        public const string ApproveCandidateInDepartureDay = "Cannot approve in the day of departure.";

        public const string InvalidUsernameLength = "Value for username should be between 2 and 20 characters.";
        public const string InvalidFirstNameLength = "Value for First Name should be between 2 and 20 characters.";
        public const string InvalidLastNameLength = "Value for Last Name should be between 2 and 20 characters.";

        public const string UsernameExists = "Username already exists.";
        public const string EmailExists = "Email already exists.";
        public const string EmailIncorrect = "Email is not valid.";
        public const string PasswordSpecialCharacters = "Password has no special characters.";
        public const string PasswordRequired = "Password is required.";

        public const string EntityNotFound = "Entity not found.";
        public const string InvalidDeparture = "Departure date should be in the future!";
        public const string InvalidFreeSpotsNumber = "Free spots cannot be less than one!";

        public const string TripDeleted = "Trip has already been deleted.";
        public const string UserDeleted = "User has already been deleted.";

        public const string UserBlocked = "This user is blocked!";
        public const string UserAlreadyBlocked = "User has already been blocked.";

        public const string RatingRequired = "Rating is required.";

        public const string InvalidFilter = "This filter is not valid.";
        public const string InvalidAction = "This action is not valid.";

        public const string NoNudeAvatars = "No nude avatars please!";
    }
}
