namespace Zambon.Core.Module.Xml.Views.SubViews
{
    /// <summary>
    /// Post back parameters informed when opening/submiting LookUpViews.
    /// </summary>
    public class PostBackOptions
    {

        /// <summary>
        /// The post back action name.
        /// </summary>
        public string PostbackActionName { get; set; }

        /// <summary>
        /// The Id of the form where should be posted the information.
        /// </summary>
        public string PostbackFormId { get; set; }

    }
}