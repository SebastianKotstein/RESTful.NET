using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Attributes
{
    /// <summary>
    /// The <see cref="Path"/> attribute closes the gap between a REST function, which is described and uniquely specified by a combination of its Uniform Ressource Locator (URL, or more specific, the path segment of this URL) and the underlying HTTP method describing 
    /// the behavior of this function, and a C# method. By applying this Path attribute to a method, a conceptually planed REST function is mapped to the corresponding method which means that an incoming HTTP request designated for this REST function will be routed to this method.
    /// Note that the signature of all REST functions (i.e. the combination of URL path segments and HTTP methods) must be disjunct which means that a certain Path attribute can only be applied to at most one method (except if a wildcard element is used). But it is possible that mulitple but disjunct Path attributes (i.e. REST functions) can be applied to a single
    /// method sharing the logic of those REST functions (in other words: this attribute can be multiply applied to a single method).
    /// 
    /// For an appropriate mapping between REST and C# code, the methods must fulfill the following preconditions:
    /// 1. The method must be a member of a <see cref="SKotstein.Net.Http.Core.HttpController"/> super type class.
    /// 2. The method must have a public modifier
    /// 3. The return type is void (all in all, it does not matter, but void is recommended since the underlying code does not process a return value)
    /// 4. The first parameter must be type of <see cref="HttpContext"/>
    /// 5. Additional parameters are not allowed unless the URL path segment defines some variable parts:
    /// 
    /// It is the common nature of REST that server-sided ressources might change during runtime and especially the URL structure might vary. For satisfying this issue, the frameworks allows to declare variable parts within the URL path segments such that a method
    /// can mapped to a URL with a variable path. Curly parentheses mark a variable part within a URL path segment (e.g. /{id}/application/{name} where id and name are variable parts). The effective value of these variables of an incoming HTTP request are passed 
    /// via parameters of the method. Make sure that the method the number of additional parameters being type of string matches the number of variable parts declared in the URL path segment.
    /// Example:
    /// <code>
    /// Path[("/app/{id}/example/{name}",HttpMethod.GET)]
    /// public void Example(HttpContext ctx, string firstVariable, string secondVariable)
    /// {
    /// 
    /// }
    /// </code>
    /// 
    /// Furthermore, it is possible to add a wildcard segment as a sufix to the URL, which means that abritrary many path segments can follow. Example: /contact/* , /contact/test/a would match for instance.
    /// Note that paths with wildcards segment can be conjunct to other paths.
    /// 
    /// Make sure that the combinations of HTTP method and URL path segments must allways be disjunct, even if variable parts are included. GET /{id} and GET /static won't be distinct for instance since "static" could be the variable value for id or the static URL path segment could be addressed,
    /// whereas GET /{id} and GET /static/{name} are distinct since an HTTP request hwith GET /1 can be exactly mapped to GET /{id}.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple=true)]
    public class Path : System.Attribute
    {
        private string _url;
        private HttpMethod _method;

        /// <summary>
        /// Creates a Path attribute consisting of a path segment of the URL and the HTTP method.
        /// </summary>
        /// <param name="url">path segment</param>
        /// <param name="method">HTTP method</param>
        public Path(string url, HttpMethod method)
        {
            _url = url;
            _method = method;
        }

        /// <summary>
        /// Gets the path segment
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
            }
        }

        /// <summary>
        /// Gets the HTTP method
        /// </summary>
        public HttpMethod Method
        {
            get
            {
                return _method;
            }
        }
    }
}
