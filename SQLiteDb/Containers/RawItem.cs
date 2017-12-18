using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteDb.Containers
{
    public class RawItem
    {
        private String _Pan;
        private decimal _Amount;

        public string Pan
        {
            get
            {
                return _Pan;
            }
            set
            {
                _Pan = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }

        public RawItem()
        {
        }

        public RawItem(string pan, decimal amount)
        {
            this._Pan = pan;
            this._Amount = amount;
        }

        public override string ToString()
        {
            return string.Format("Pan: {0}, Amount: {1}", _Pan, _Amount);
        }
    }
}
