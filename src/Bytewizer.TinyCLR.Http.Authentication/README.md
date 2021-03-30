# Authentication

Enables implementation of basic and digest authentication.

>**_Caution_** Data sent with Basic and Digest Authentication is not encrypted so the data can be seen by an adversary. Additionally, Basic Authentication credentials are sent in the clear and can be intercepted. The the body of the HTTP communication is still in plaintext. To secure the body of your communication you should use Secure Sockets Layer (SSL).

## Digest Authentication Example
```CSharp
var accountService = new DefaultAccountService();
accountService.Register("admin", "password");

var server = new HttpServer(options =>
{
    options.Pipeline(app =>
    {
        app.UseRouting();
        app.UseAuthentication(accountService);
        app.UseEndpoints(endpoints =>
        {
            endpoints.Map("/", context =>
            {
                var username = context.GetCurrentUser().Username;
                context.Response.Write(username);
            });
        });
    });
});
server.Start();
```

## Basic Authentication Example
```CSharp
var accountService = new DefaultAccountService();
accountService.Register("admin", "password");

var server = new HttpServer(options =>
{
    options.Pipeline(app =>
    {
        app.UseRouting();
        app.UseAuthentication(new AuthenticationOptions
        {
            AuthenticationProvider = new BasicAuthenticationProvider(),
            AccountService = accountService
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.Map("/", context =>
            {
                var username = context.GetCurrentUser().Username;
                context.Response.Write(username);
            });
        });
    });
});
server.Start();
```

## Custom IAccountService Authentication Example

```CSharp
public class AccountService : IAccountService
{
    private readonly Hashtable _users = new Hashtable();

    public AccountService()
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        _users.Add("bsmith", new User
        {
            Id = 1,
            FirstName = "Bob",
            LastName = "Smith",
            Username = "bsmith",
            HA1 = AuthHelper.ComputeA1Hash("bsmith", "password")
        });
        _users.Add("ksmith", new User
        {
            Id = 2,
            FirstName = "Kim",
            LastName = "Smith",
            Username = "ksmith",
            HA1 = AuthHelper.ComputeA1Hash("ksmith", "password")
        });
    }

    public IUser GetUser(string username)
    {
        if (_users.Contains(username))
        {
            // authentication successful so return user details
            return (IUser)_users[username];
        }

        // return null if user not found
        return null;
    }
}
```
```CSharp
public class User : IUser
{
    public User(int id, string firstName, string lastName, string username, string ha1)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        HA1 = ha1;
    }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string HA1 { get; set; }
}
```
```CSharp
var accountService = new AccountService();
accountService.Register("admin", "password");

var server = new HttpServer(options =>
{
    options.Pipeline(app =>
    {
        app.UseRouting();
        app.UseAuthentication(accountService);
        app.UseEndpoints(endpoints =>
        {
            endpoints.Map("/", context =>
            {
                var username = context.GetCurrentUser().Username;
                context.Response.Write(username);
            });
        });
    });
});
server.Start();
```

## RFC - Related Request for Comments 
- [RFC 2617 - HTTP Authentication: Basic and Digest Access Authentication](https://tools.ietf.org/html/rfc2617)
