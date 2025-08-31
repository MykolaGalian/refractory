using Microsoft.AspNetCore.Identity;

namespace RefractoryApp.DAL.Identity;

// Кастомные классы Identity с int ключами (как в старом проекте)
public class ApplicationUserRole : IdentityUserRole<int> { }
public class ApplicationUserClaim : IdentityUserClaim<int> { }
public class ApplicationUserLogin : IdentityUserLogin<int> { }
public class ApplicationRoleClaim : IdentityRoleClaim<int> { }
public class ApplicationUserToken : IdentityUserToken<int> { }