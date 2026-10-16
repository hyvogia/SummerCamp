using HyVo_Proj1SummerCampXF.Data;
using HyVo_Proj1SummerCampXF.Models;
using HyVo_Proj1SummerCampXF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HyVo_Proj1SummerCampXF
{
    public partial class MainPage : ContentPage
    {
        private App thisApp;
        private List<Compound> compounds;

        public MainPage()
        {
            InitializeComponent();
            thisApp = Application.Current as App;
            thisApp.needCamperRefresh = true;
            compounds = new List<Compound>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            btnAdd.IsEnabled = false;

            await ShowCompounds();
            if (thisApp.needCamperRefresh)
            {
                RefreshCampers();
            }
            else
            {
                camperList.IsVisible = true;
            }
            camperList.SelectedItem = null;

            btnAdd.IsEnabled = true;
        }

        private async Task ShowCompounds()
        {
            if (!(thisApp.AllCompounds?.Count > 0))
            {
                CompoundRepository atr = new CompoundRepository();
                try
                {

                    compounds.Add(new Compound { ID = 0, Name = " All Compounds" });
                    thisApp.AllCompounds = await atr.GetCompounds();
                    foreach (Compound p in thisApp.AllCompounds.OrderBy(d => d.Name))
                    {
                        compounds.Add(p);
                    }

                    ddlCompounds.ItemsSource = compounds;
                    ddlCompounds.SelectedIndex = 0;
                }
                catch (ApiException apiEx)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Errors:");
                    foreach (var error in apiEx.Errors)
                    {
                        sb.AppendLine("-" + error);
                    }
                    thisApp.needCamperRefresh = true;
                    await DisplayAlert("Problem Getting List of Compounds:", sb.ToString(), "Ok");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.GetBaseException().Message.Contains("connection with the server"))
                        {

                            await DisplayAlert("Error", "No connection with the server. Check that the Web Service is running and available and then click the Refresh button.", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Error", "If the problem persists, please call your system administrator.", "Ok");
                        }
                    }
                    else
                    {
                        if (ex.Message.Contains("NameResolutionFailure"))
                        {
                            await DisplayAlert("Internet Access Error ", "Cannot resolve the Uri: " + Jeeves.DBUri.ToString(), "Ok");
                        }
                        else
                        {
                            await DisplayAlert("General Error ", ex.Message, "Ok");
                        }
                    }
                }
            }
        }

        private async void ShowCampers(int? CompoundID)
        {
            CamperRepository r = new CamperRepository();
            try
            {
                List<Camper> campers;
                if (CompoundID.GetValueOrDefault() > 0)
                {
                    campers = await r.GetCampersByCompound(CompoundID.GetValueOrDefault());
                }
                else
                {
                    campers = await r.GetCampers();
                }
                camperList.ItemsSource = campers;
                camperList.IsVisible = true;
            }
            catch (ApiException apiEx)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Errors:");
                foreach (var error in apiEx.Errors)
                {
                    sb.AppendLine("-" + error);
                }
                camperList.IsVisible = false;
                await DisplayAlert("Error Getting Campers:", sb.ToString(), "Ok");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.GetBaseException().Message.Contains("connection with the server"))
                    {

                        await DisplayAlert("Error", "No connection with the server. Check that the Web Service is running and available and then click the Refresh button.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Error", "If the problem persists, please call your system administrator.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("General Error", "If the problem persists, please call your system administrator.", "Ok");
                }
            }
        }

        private void ddlCompounds_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCampers();
        }

        private void RefreshCampers()
        {
            thisApp = Application.Current as App;
            if (ddlCompounds.SelectedIndex < 1)
            {
                ShowCampers(null);
            }
            else
            {
                Compound selCompound = (Compound)ddlCompounds.SelectedItem;
                ShowCampers(selCompound.ID);
            }
            thisApp.needCamperRefresh = false;
        }

        private void CamperSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var camper = (Camper)e.SelectedItem;
                var camperDetailPage = new CamperDetailsPage();
                camperDetailPage.BindingContext = camper;
                camperList.IsVisible = false;
                Navigation.PushAsync(camperDetailPage);
            }
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {
            Camper newCamper = new Camper();
            var camperDetailPage = new CamperDetailsPage();
            camperDetailPage.BindingContext = newCamper;
            camperList.IsVisible = false;
            Navigation.PushAsync(camperDetailPage);
        }
    }
}
