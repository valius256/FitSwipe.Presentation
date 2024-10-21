
using FitSwipe.Shared.Enums;
using System.Data;
using System.Net.NetworkInformation;

namespace FitSwipe.Shared.Dtos.Users
{
  public class GetUserDto
  {
    public Guid Id { get; set; }
    public string FireBaseId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string? Email { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public string? Bio { get; set; }
    public string City { get; set; } = string.Empty;
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public Role Role { get; set; }
    public string Job { get; set; } = string.Empty;
    public UserStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    public RecordStatus RecordStatus { get; set; }
    public string AddressString
    {
      get => City;
    }
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
  }
}
