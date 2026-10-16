using HyVo_Proj1SummerCampXF.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HyVo_Proj1SummerCampXF
{
    public partial class App : Application
    {
        public List<Compound> AllCompounds;
        public bool needCamperRefresh;

        public App()
        {
            InitializeComponent();

            NavigationPage nav = new NavigationPage(new MainPage());
            MainPage = nav;

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
