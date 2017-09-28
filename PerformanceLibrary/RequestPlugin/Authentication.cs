using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceLibrary.RequestPlugin
{
    public class Authentication : WebTestRequestPlugin
    {
        #region Properties


        #endregion

        #region Methods

        public override void PreRequestDataBinding(object sender, PreRequestDataBindingEventArgs e)
        {
            base.PreRequestDataBinding(sender, e);
        }
        #endregion
    }
}
