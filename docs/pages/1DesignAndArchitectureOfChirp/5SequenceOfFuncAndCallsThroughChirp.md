<!-- With a UML sequence diagram, illustrate the flow of messages and data through your Chirp! application. Start with an HTTP request that is send by an unauthorized user to the root endpoint of your application and end with the completely rendered web-page that is returned to the user.

Make sure that your illustration is complete. That is, likely for many of you there will be different kinds of "calls" and responses. Some HTTP calls and responses, some calls and responses in C\# and likely some more. (Note the previous sentence is vague on purpose. I want that you create a complete illustration. -->

UML sequence diagrams

![[../../diagrams/SimunsPics/NewUser.png]]
\
In the above diagram can be tracked the flow of a user signing up for (or in to) our service. 
The user accesses the web app by navigating to any of the sites endpoints.
This sends a ***HTTP GET*** request to the server.
The user being not yet authorised, is only presented the option of logging in - by way of a HTML response.
The user 
\
![[../../diagrams/SimunsPics/_Search.png]]
\
![[../../diagrams/SimunsPics/_Follow.png]]
\
![[../../diagrams/SimunsPics/_Post.png]]
\
```ruby
class Max
    public static void Mark(){

    }
```