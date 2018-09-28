# Umbraco Analyzers
Roslyn Analyzers and CodeFixes to help guide users building websites with Umbraco inside the Visual Studio IDE

## Ideas to implement
- [ ] Avoid useage of Singlton access of UmbracoContext or ApplicationContext - especially when it is in a base class
- [ ] If GetAncestors() chained with GetDescendants() - throw a Warning. ARE YOU SURE? *Can we monitor RAZOR files?*
- [ ] Do not assign umbracoHelper to a private static - it's Request based & should not be stored into Application lifecycle
```
//EXAMPLES OF HOW NOT TO USE - NOT SURE WE CAN CODE FIX THIS THOUGH?
private static _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

private static _umbracoContext = UmbracoContext.Current;

// MembershipHelper is also a request scoped instance - it relies either on an UmbracoContext or an HttpContext
private static _membershipHelper = new MembershipHelper(UmbracoContext.Current);

private static _request = HttpContext.Current.Request;
```
- [ ] API Service access in views (May only be able to map certain method calls to UmbracoHelper calls)
```
// Services access in your views :(
var dontDoThis = ApplicationContext.Services.ContentService.GetById(123);

// Content cache access in your views :)
var doThis = Umbraco.TypedContent(123);
```
