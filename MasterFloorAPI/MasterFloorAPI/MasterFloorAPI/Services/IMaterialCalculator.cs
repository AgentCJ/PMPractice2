namespace MasterFloorAPI.Services
{
    public interface IMaterialCalculator
    {
        Task<int> CalculateMaterialAsync(int productTypeId, int materialId, int quantity, double width, double length);
    }
}