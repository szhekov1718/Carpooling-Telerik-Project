<img src="https://webassets.telerikacademy.com/images/default-source/logos/telerik-academy.svg)" alt="logo" width="300px" style="margin-top: 20px;"/>

# Carpooling Project Assignment  

<p align="center">

## **Project Description**
Carpooling is a web application that enables you to share your travel from one location to other with other passengers. Every user can either organize a shared travel or request to join someone else’s travel. 


## **Technologies Used**
- .NET Core
- ASP.NET MVC
- ASP.NET API
- Swagger
- MS SQL Server
- Entity Framework Core
- HTML5
- CSS3
- Bootstrap
- Moq
- LINQ

Teamwork and communication: GitLab with separate branches, Git Lab Issues and board, MS Teams.

Best programming practices and principles used: OOP, SOLID, and KISS principles, client-side  and server-side data validation, exception handling, unit testing of the "business" functionality, etc.


## **General Information**

Our carpooling solution includes 5 projects:

- Carpooling.Data 
- Carpooling.Services 
- Carpooling.API
- Carpooling.MVC
- Carpooling.Tests


## **Authors**
- [Stanimir Zhekov](https://www.linkedin.com/in/stanimir-zhekov/)
- Stanislav Staykov


## External Services

Carpooling uses the following external service:

- **DeepAI (API)**  
  https://deepai.org/


## **Public Part**

The public part is accessible without authentication i.e., for anonymous users. Anonymous users can see information about Carpooling and its features as well as how many people are using the platform and how many travels have happened.

Our home page looks like this:

![HomePage](Screenshots/homePage.png)

And that is our about page:

![AboutPage](Screenshots/aboutPage.png)

Anonymous users can register: 

![Register](Screenshots/registerForm.png)

They can also login in their existing profile:

![Login](Screenshots/loginForm.png)

## **Private Part**

Accessible only if the user is authenticated. Users can login/logout, update their profile and set a profile photo.

![UpdateProfile](Screenshots/updateProfile.png)

Each user can create a new travel that he is planning:

![CreateTrip](Screenshots/createTrip.png)

Each user can browse the available trips created by other users:

![AllTrips](Screenshots/trips.png)

The user can also sort and filter them:

![FilterTrips](Screenshots/tripsFilter.png)

Each user can apply for a trip as a passenger: 

![ApplyForTrip](Screenshots/applyTrip.png)

The driver can approve/decline passengers from the candidates’ pool:

![ApproveDeclineCandidate](Screenshots/approveCandidates.png)

The driver can cancel a trip before the departure time:

![CancelTrip](Screenshots/cancelTrip.png)

The driver can also mark the trip as complete:

![CompleteTrip](Screenshots/tripCompleted.png)

Passengers can leave feedback about the driver as well as the driver can leave feedback for every passenger. Feedback includes numeric rating (from 0 to 5) and optional text comment:

![Comments](Screenshots/myComments.png)

Each user can view all his travels and all feedback:

![MyTrips](Screenshots/myTrips.png)

The user can also filter and sort them:

![MyTripsFilter](Screenshots/myTripsFilter.png)

## **Administrative Part**

Accessible to users with administrative privileges. Admin users can see list of all users and block or unblock them. A blocked user cannot create travels and apply for travels.

![AllUsers](Screenshots/allUsers.png)

The admin can search users by phone number, username or email:

![UsersFilter](Screenshots/usersFilter.png)


## **EXTRA Features**

✅ Users cannot upload inappropriate profile photos, because we have Nudity Detection!

![NudityDetection](Screenshots/nudeAvatars.png)

✅ Users cannot use swear words when they are leaving comments!

![SwearWordsFilter](Screenshots/beforeSwear.png)

![SwearWordsFilterWorking](Screenshots/afterSwear.png)

✅ Every new user receives a welcome email.

![WelcomeEmail](Screenshots/welcomeEmail.png)

✅ Customers can contact us with questions or leave us feedback.

![ContactForm](Screenshots/contactForm.png)


## **Database Diagram**

![DatabaseDiagram](Screenshots/databaseDiagram.png)


## **API and Swagger Documentation**
The API supports all the functionalities of the web part of the application.

![SwaggerPart1](Screenshots/swaggerPart1.png)
![SwaggerPart2](Screenshots/swaggerPart2.png)

