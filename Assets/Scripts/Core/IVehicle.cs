public interface IVehicle
{
    float CurrentSpeed { get; }
    float NormalizedSpeed { get; }
    bool IsGrounded { get; }
    bool IsBoosting { get; }
}
