using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews
{
    /// <summary>
    /// Represents a node <PaginationOptions></PaginationOptions> from XML Application Model.
    /// </summary>
    public class PaginationOptions : XmlNode
    {
        /// <summary>
        /// The PageSize attribute from XML. The maximum number of records to display in a same page.
        /// </summary>
        [XmlAttribute("PageSize"), Browsable(false)]
        public string IntPageSize
        {
            get { return PageSize.ToString(); }
            set { int.TryParse(value, out int pageSize); PageSize = pageSize; }
        }
        /// <summary>
        /// The PageSize attribute from XML. The maximum number of records to display in a same page.
        /// </summary>
        [XmlIgnore]
        public int? PageSize { get; set; }

        /// <summary>
        /// The PagesToShow attribute from XML. The maximum number of pages to show in footer.
        /// </summary>
        [XmlAttribute("PagesToShow"), Browsable(false)]
        public string IntPagesToShow
        {
            get { return PagesToShow?.ToString(); }
            set { int.TryParse(value, out int pagesToShow); PagesToShow = pagesToShow; }
        }
        /// <summary>
        /// The PagesToShow attribute from XML. The maximum number of pages to show in footer.
        /// </summary>
        [XmlIgnore]
        public int? PagesToShow { get; set; }

        /// <summary>
        /// The actual page being displayed in view.
        /// </summary>
        [XmlIgnore]
        public int? ActualPage { get; private set; }

        /// <summary>
        /// The total amount of pages.
        /// </summary>
        [XmlIgnore]
        public int? TotalPages { get; private set; }

        /// <summary>
        /// The start page begin displayed in pages footer.
        /// </summary>
        [XmlIgnore]
        public int? StartPage { get; private set; }

        /// <summary>
        /// The last page begin displayed in pages footer.
        /// </summary>
        [XmlIgnore]
        public int? EndPage { get; private set; }

        /// <summary>
        /// The total amount of records available in view.
        /// </summary>
        [XmlIgnore]
        public int? TotalRecords { get; private set; }

        #region Methods

        /// <summary>
        /// Changes the page to the informed page and recalculates start and end pages.
        /// </summary>
        /// <param name="newPage">The new page to set the view.</param>
        /// <param name="totalRecords">The total amount of records.</param>
        public void ChangePage(int newPage, int totalRecords)
        {
            var changedTotalRecords = false;
            var changedActualPage = false;

            if (!totalRecords.Equals(TotalRecords))
            {
                TotalPages = Convert.ToInt32(Math.Ceiling((decimal)totalRecords / (decimal)PageSize));

                if (TotalPages < 1)
                    TotalPages = 1;

                TotalRecords = totalRecords;
                changedTotalRecords = true;
            }

            if (!ActualPage.Equals(newPage))
            {
                if (TotalPages == 0) TotalPages = 1;

                ActualPage = (newPage <= 0 ? 1 : newPage > TotalPages ? TotalPages : newPage);
                changedActualPage = true;
            }

            if (changedActualPage || changedTotalRecords)
            {
                StartPage = 1;
                if (TotalPages <= PagesToShow)
                    EndPage = TotalPages;
                else
                {
                    StartPage = ActualPage - Convert.ToInt32(Math.Ceiling((double)PagesToShow / 2));
                    EndPage = ActualPage + Convert.ToInt32(Math.Ceiling((double)PagesToShow / 2));

                    if (StartPage <= 0)
                    {
                        EndPage += (StartPage * -1) + 1;
                        StartPage = 1;
                    }
                    else if (EndPage > TotalPages - 1)
                    {
                        StartPage += TotalPages - EndPage;
                        EndPage = TotalPages;
                    }
                }
            }
        }

        #endregion

    }
}