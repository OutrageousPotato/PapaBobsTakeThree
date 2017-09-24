using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PapaBobs.DTO.Enums;

namespace PapaBobs.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void orderButton_Click(object sender, EventArgs e)
        {

            if (nameTextBox.Text.Trim().Length == 0)
            {
                ValidationLabel.Text = "Please enter a name.";
                ValidationLabel.Visible = true;
                return;
            }

            if (addressTextBox.Text.Trim().Length == 0)
            {
                ValidationLabel.Text = "Please enter address.";
                ValidationLabel.Visible = true;
                return;
            }

            if (zipTextBox.Text.Trim().Length == 0)
            {
                ValidationLabel.Text = "Please enter a zip code.";
                ValidationLabel.Visible = true;
                return;
            }

            if (phoneTextBox.Text.Trim().Length == 0)
            {
                ValidationLabel.Text = "Please enter a phone number.";
                ValidationLabel.Visible = true;
                return;
            }

            try
            {
                var order = buildOrder();
                Domain.OrderManager.CreateOrder(order);
                Response.Redirect("Success.aspx");
            }
            catch (Exception ex)
            {
                ValidationLabel.Text = ex.Message;
                ValidationLabel.Visible = true;
                return;
            }

        }

        private PaymentType determinePaymentType()
        {
            DTO.Enums.PaymentType paymentType;
            if (cashRadioButton.Checked)
            {
                paymentType = DTO.Enums.PaymentType.Cash;
            }
            else
            {
                paymentType = DTO.Enums.PaymentType.Credit;
            }

            return paymentType;
        }

        private CrustType determineCrust()
        {
            DTO.Enums.CrustType crust;
            if (!Enum.TryParse(crustDropDownList.SelectedValue, out crust))
            {
                throw new Exception("Could not determine pizza crust.");
            }

            return crust;
        }

        private DTO.Enums.SizeType determineSize()
        {
            DTO.Enums.SizeType size;
            if (!Enum.TryParse(sizeDropDownList.SelectedValue, out size))
            {
                throw new Exception("Could not determine pizza size.");
            }

            return size;
        }

        protected void recalculateTotalCost(object sender, EventArgs e)
        {
            if (sizeDropDownList.SelectedValue == String.Empty) return;
            if (crustDropDownList.SelectedValue == String.Empty) return;
            var order = buildOrder();

            try
            {
                totalLabel.Text = Domain.PizzaPriceManager.CalculateCost(order).ToString("C");
            }
            catch
            {
                // Swallow the error
            }
        }




        private DTO.OrderDTO buildOrder()
        {
            var order = new DTO.OrderDTO();
            order.Size = determineSize();
            order.Crust = determineCrust();
            order.Sausage = sausageCheckBox.Checked;
            order.Pepperoni = pepperoniCheckBox.Checked;
            order.Onion = onionCheckBox.Checked;
            order.GreenPeppers = greenPeppersCheckBox.Checked;
            order.Name = nameTextBox.Text;
            order.Address = addressTextBox.Text;
            order.Zip = zipTextBox.Text;
            order.Phone = phoneTextBox.Text;
            order.PaymentType = determinePaymentType();

            return order;
        }

    }
}