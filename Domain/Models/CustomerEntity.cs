using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Customers")]
    public class CustomerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CustomerId")]
        public virtual Guid Id { get; set; }

        [Required(ErrorMessage = "FirstNameRequiredError")]
        public virtual string FirstName { get; set; }

        [Required(ErrorMessage = "LastNameRequiredError")]
        public virtual string LastName { get; set; }

        [Phone(ErrorMessage = "PhoneNumberInvalidError")]
        public virtual string PhoneNumber { get; set; }

        public virtual string Address { get; set; }

        public virtual string City { get; set; }

        public virtual string State { get; set; }

        public virtual int ZipCode { get; set; }

        public bool ValidateCustomerEntity()
        {
            return !(Id == Guid.Empty)
                && !string.IsNullOrEmpty(FirstName)
                && !string.IsNullOrEmpty(LastName);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var customerPassedIn = obj as CustomerEntity;
            if (customerPassedIn == null)
            {
                return false;
            }

            if (Id != customerPassedIn.Id
                || string.CompareOrdinal(FirstName, customerPassedIn.FirstName) != 0
                || string.CompareOrdinal(LastName, customerPassedIn.LastName) != 0
                || string.CompareOrdinal(PhoneNumber, customerPassedIn.PhoneNumber) != 0
                || string.CompareOrdinal(Address, customerPassedIn.Address) != 0
                || string.CompareOrdinal(City, customerPassedIn.City) != 0
                || string.CompareOrdinal(State, customerPassedIn.State) != 0
                || ZipCode != customerPassedIn.ZipCode)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var result = 0x2D2816FE;

            result = result * 31 + Id.GetHashCode();
            result = result * 31 + FirstName?.GetHashCode() ?? 0;
            result = result * 31 + LastName?.GetHashCode() ?? 0;
            result = result * 31 + PhoneNumber?.GetHashCode() ?? 0;
            result = result * 31 + Address?.GetHashCode() ?? 0;
            result = result * 31 + City?.GetHashCode() ?? 0;
            result = result * 31 + State?.GetHashCode() ?? 0;
            result = result * 31 + (ZipCode == 0 ? 0 : ZipCode.GetHashCode());

            return result;
        }
    }
}