using HyVo_Proj1SummerCampXF.Data;
using HyVo_Proj1SummerCampXF.Models;
using HyVo_Proj1SummerCampXF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HyVo_Proj1SummerCampXF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CamperDetailsPage : ContentPage
    {
        private Camper camper;
        private App thisApp;
        private List<Compound> compounds;

        public CamperDetailsPage()
        {
            InitializeComponent();
            thisApp = Application.Current as App;
            compounds = new List<Compound>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            camper = (Camper)this.BindingContext;
            if (camper.ID == 0)
            {
                this.Title = "Add New Camper";
                Compound noCompound = new Compound { ID = 0, Name = " Select a Compound" };
                compounds.Add(noCompound);
                btnDelete.IsEnabled = false;
            }
            else
            {
                this.Title = "Edit Camper Details";
                btnDelete.IsEnabled = true;
            }

            FillCompound();
        }

        private void FillCompound()
        {
            foreach (Compound d in thisApp.AllCompounds.OrderBy(d => d.Name))
            {
                compounds.Add(d);
            }
            //ddl
            ddlCompounds.ItemsSource = compounds;
            //set current value
            if (camper.CompoundID >= 0)
            {
                ddlCompounds.SelectedItem = thisApp.AllCompounds.FirstOrDefault(d => d.ID == camper.CompoundID);
            }
        }

        private async void SaveClicked(object sender, EventArgs e)
        {
            try
            {
                camper.Compound = (Compound)ddlCompounds.SelectedItem;
                camper.CompoundID = (camper.Compound?.ID).GetValueOrDefault();

                if (camper.CompoundID > 0)
                {
                    CamperRepository r = new CamperRepository();
                    if (camper.ID == 0)
                    {
                        await r.AddCamper(camper);
                    }
                    else
                    {
                        await r.UpdateCamper(camper);
                    }
                    thisApp.needCamperRefresh = true;
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Compound Not Selected:", "You must set a Compound for the camper.", "Ok");
                }

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
                await DisplayAlert("Problem Saving the Camper:", sb.ToString(), "Ok");
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    await DisplayAlert("Error", "No connection with the server.", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Could not complete operation.", "Ok");
                }
            }
        }

        private async void DeleteClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirm Delete", "Are you certain that you want to delete " + camper.FullName + "?", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    CamperRepository er = new CamperRepository();
                    await er.DeleteCamper(camper);
                    thisApp.needCamperRefresh = true;
                    await Navigation.PopAsync();
                }
                catch (ApiException apiEx)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Errors:");
                    foreach (var error in apiEx.Errors)
                    {
                        sb.AppendLine("-" + error);
                    }
                    await DisplayAlert("Problem Deleting the Camper:", sb.ToString(), "Ok");
                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException().Message.Contains("connection with the server"))
                    {
                        await DisplayAlert("Error", "No connection with the server.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Could not complete operation.", "Ok");
                    }
                }
            }
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}