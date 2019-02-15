﻿using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.ListViews
{
    public class PaginationOptions : XmlNode
    {
        [XmlAttribute("PageSize")]
        public int PageSize { get; set; }

        [XmlAttribute("PagesToShow")]
        public int PagesToShow { get; set; }

        [XmlIgnore]
        public int ActualPage { get; private set; }

        [XmlIgnore]
        public int TotalPages { get; private set; }

        [XmlIgnore]
        public int StartPage { get; private set; }

        [XmlIgnore]
        public int EndPage { get; private set; }

        [XmlIgnore]
        public int TotalRecords { get; private set; }

        #region Methods

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