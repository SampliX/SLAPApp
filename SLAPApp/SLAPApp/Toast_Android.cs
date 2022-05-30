using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SLAPApp.ToastX))]

namespace SLAPApp
{
    internal class ToastX : IToast
    {
        public void Show(string message)
        {
            DependencyService.Get<IToast>().Show(message);
        }
    }
}
