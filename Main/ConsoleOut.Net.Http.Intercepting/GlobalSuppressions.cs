// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Invalid sugggestion, since DI is going to need to access the collection.", Scope = "member", Target = "~P:ConsoleOut.Net.Http.Intercepting.HttpInterceptorOptions.Headers")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Don't care.", Scope = "type", Target = "~T:ConsoleOut.Net.Http.Intercepting.HttpInterceptorOptions.HttpResponseHeader")]