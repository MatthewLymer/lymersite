using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Lymer.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Render a partial for a model property
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expression">Expression for the property of the model you want to pass to the partial view</param>
        /// <param name="partialViewName">Name of the partial view</param>
        /// <returns>Contents of the rendered partial</returns>
        public static MvcHtmlString PartialFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper, 
            Expression<Func<TModel, TProperty>> expression, 
            string partialViewName)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            object model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;

            ViewDataDictionary viewData = new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = new TemplateInfo
                {
                    HtmlFieldPrefix = name
                }
            };

            return helper.Partial(partialViewName, model, viewData);
        }
    }
}
