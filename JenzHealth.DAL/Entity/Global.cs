using JenzHealth.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Global
    {

        public static int AuthenticatedUserID { get; set; }
        public static int AuthenticatedUserRoleID { get; set; }
        public static string ErrorMessage { get; set; }

        public static string ReturnUrl { get; set; }
    
    }
    public class EnumDisplayNameAttribute : Attribute
    {
        private string _displayName;
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
            }
        }
    }

    public static class EnumExtensions
    {

        public static string DisplayName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            EnumDisplayNameAttribute attribute
                = Attribute.GetCustomAttribute(field, typeof(EnumDisplayNameAttribute))
            as EnumDisplayNameAttribute;

            return attribute == null ? value.ToString() : attribute.DisplayName;
        }
    }

    public enum CustomerType
    {
        [EnumDisplayName(DisplayName = "Registered Customer")]
        REGISTERED_CUSTOMER = 1,

        [EnumDisplayName(DisplayName = "Walk-In Customer")]
        WALK_IN_CUSTOMER
    }
    public enum WaiveBy
    {
        [EnumDisplayName(DisplayName = "Waive By Amount")]
        AMOUNT = 1,

        [EnumDisplayName(DisplayName = "Waive By Percentage")]
        PERCENTAGE
    }
    public enum PaymentType
    {
        [EnumDisplayName(DisplayName = "Cash Deposite")]
        CASH = 1,

        [EnumDisplayName(DisplayName = "Point Of Service")]
        POS,

        [EnumDisplayName(DisplayName = "Electronic Funds Transfer")]
        EFT
    }

    public enum RecieptTypes
    {
        [EnumDisplayName(DisplayName = "Payment Reciept")]
        PAYMENT_RECIEPT = 1,
        [EnumDisplayName(DisplayName = "Deposite Recipet")]
        DEPOSITE_RECIEPT,
    }
    public enum CollectionType
    {
        [EnumDisplayName(DisplayName = "Billed")]
        BILLED = 1,
        [EnumDisplayName(DisplayName = "Unbilled")]
        UNBILLED,
        [EnumDisplayName(DisplayName = "Walk-In Customer")]
        WALK_IN
    }
    public enum MicroscopyType
    {
        [EnumDisplayName(DisplayName = "N/A")]
        NA = 1,
        [EnumDisplayName(DisplayName = "Wet mount")]
        WET_MOUNT,
        [EnumDisplayName(DisplayName = "Stain")]
        STAIN,
        [EnumDisplayName(DisplayName = "SFA")]
        SFA,
        [EnumDisplayName(DisplayName = "KOH")]
        KOH,
        [EnumDisplayName(DisplayName = "Others")]
        OTHERS
    }
    public enum StainType
    {
        [EnumDisplayName(DisplayName = "Grain Stain")]
        GRAIN_STAIN = 1,
        [EnumDisplayName(DisplayName = "Giemsa Stain")]
        GIEMSA_STAIN,
        [EnumDisplayName(DisplayName = "Ziehl Neelson Stain")]
        ZIEHL_NEELSON_STAIN,
        [EnumDisplayName(DisplayName = "India Ink Stain")]
        INDIAN_INK_STAIN,
        [EnumDisplayName(DisplayName = "Iodine Stain")]
        IODINE_STAIN,
        [EnumDisplayName(DisplayName = "Methylene Blue Stain")]
        METHYLENE_BLUE_STAIN,
        [EnumDisplayName(DisplayName = "Others")]
        OTHERS
    }
}
