using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{

    public class AddBoardofDirectorsViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase File { get; set; } = null;
    }
    public class DisplayBoardOfDirectorsViewModel
    {
        private int _Ranking;
        private string _Title;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title
        {
            get
            {
                return _Title;
            }

            set
            {
                _Title = value;
            }
        }
        public string Email { get; set; }
        public string Description { get; set; }
        public byte[] PhotoContent { get; set; }
        public int Rank
        {
            get
            {
                return _Ranking;
            }
            set
            {
                if (_Title == "President")
                    _Ranking = 1;
                else if (_Title == "Vice President")
                    _Ranking = 2;
                else if (_Title == "Executive Director")
                    _Ranking = 3;
                else if (_Title == "Treasurer")
                    _Ranking = 4;
                else if (_Title == "Executive Assistant")
                    _Ranking = 5;
                else if (_Title == "Owner Director")
                    _Ranking = 6;
                else if (_Title == "Trainer Director")
                    _Ranking = 7;
                else if (_Title == "Alternate Trainer Director")
                    _Ranking = 8;
                else
                    _Ranking = 9;
            }
        }
    }
}