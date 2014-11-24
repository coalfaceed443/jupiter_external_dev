using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM.Code.Utils.Validation
{
    public enum ValidationDataOptions
    {
        Double,
        Integer,
        Currency,
        String,
        Date,
        None
    }
    
    public class Validation
    {
        public static RequiredFieldValidator Required(string message, string target)
        {
            RequiredFieldValidator reqField = new RequiredFieldValidator();
            reqField.ErrorMessage = message + " is required";
            reqField.ControlToValidate = target;
            reqField.EnableClientScript = false;
            reqField.Display = ValidatorDisplay.None;
            reqField.ID = target.Replace("txt", "req");

            return reqField;
        }

        public static CompareValidator Compare(string message, string target, string target2)
        {
            CompareValidator comField = new CompareValidator();
            comField.ErrorMessage = message + " does not match";
            comField.ControlToValidate = target;
            comField.ControlToCompare = target2;
            comField.EnableClientScript = false;
            comField.Display = ValidatorDisplay.None;

            return comField;
        }

        public static CompareValidator TypeCheck(string message, string target, ValidationDataType type)
        {
            string typeMessage = "";

            switch (type)
            {
                case ValidationDataType.Double:
                    typeMessage = "a number";
                    break;
                case ValidationDataType.Integer:
                    typeMessage = "a number";
                    break;
                case ValidationDataType.String:
                    typeMessage = "text";
                    break;
            }

            CompareValidator comField = new CompareValidator();
            comField.ErrorMessage = message + " is not " + typeMessage;
            comField.ControlToValidate = target;
            comField.Operator = ValidationCompareOperator.DataTypeCheck;
            comField.Type = type;
            comField.EnableClientScript = false;
            comField.Display = ValidatorDisplay.None;

            return comField;
        }

        public static RegularExpressionValidator DateCheck(string message, string target)
        {
            return RegExpression(message, target, @"^(((((0[1-9])|(1\d)|(2[0-8]))/((0[1-9])|(1[0-2])))|((31/((0[13578])|(1[02])))|((29|30)/((0[1,3-9])|(1[0-2])))))/((20[0-9][0-9]))|((((0[1-9])|(1\d)|(2[0-8]))/((0[1-9])|(1[0-2])))|((31/((0[13578])|(1[02])))|((29|30)/((0[1,3-9])|(1[0-2])))))/((19[0-9][0-9]))|(29/02/20(([02468][048])|([13579][26])))|(29/02/19(([02468][048])|([13579][26]))))$");
        }

        public static RegularExpressionValidator TimeCheck(string message, string target)
        {
            return RegExpression(message, target, @"^([0-1][0-9]|[2][0-3]):([0-5][0-9])$");
        }

        public static RegularExpressionValidator RegExpression(string message, string target, string expression)
        {
            RegularExpressionValidator regField = new RegularExpressionValidator();
            regField.ErrorMessage = message + " is invalid";
            regField.ControlToValidate = target;
            regField.ValidationExpression = expression;
            regField.EnableClientScript = false;
            regField.Display = ValidatorDisplay.None;

            return regField;
        }

        public static CustomValidator Custom(string message, ServerValidateEventHandler sve)
        {
            CustomValidator cusField = new CustomValidator();
            cusField.ErrorMessage = message;
            cusField.ServerValidate += sve;
            cusField.EnableClientScript = false;
            cusField.Display = ValidatorDisplay.None;

            return cusField;
        }
    }
}