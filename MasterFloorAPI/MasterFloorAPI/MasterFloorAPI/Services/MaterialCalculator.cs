using MasterFloorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterFloorAPI.Services
{
    public class MaterialCalculator : IMaterialCalculator
    {
        private readonly MasterFloorContext _context;

        public MaterialCalculator(MasterFloorContext context)
        {
            _context = context;
        }

        public async Task<int> CalculateMaterialAsync(int productTypeId, int materialId, int quantity, double width, double length)
        {
            try
            {
                // Проверка валидности входных данных
                if (quantity <= 0 || width <= 0 || length <= 0)
                    return -1;

                // Проверка существования типа продукции и материала
                var productType = await _context.ProductTypes.FindAsync(productTypeId);
                var material = await _context.Materials.FindAsync(materialId);

                if (productType == null || material == null)
                    return -1;

                // Расчёт количества материала на одну единицу продукции
                double materialPerUnit = width * length * (double)productType.Coefficient;

                // Общее количество материала без учёта брака
                double totalMaterial = materialPerUnit * quantity;

                // Учёт процента брака (defect_rate – это десятичная дробь, например 0.05 для 5%)
                double defectRate = (double)material.DefectRate;
                double materialWithDefect = totalMaterial * (1 + defectRate);

                // Возвращаем целое количество (округляем вверх, чтобы материала хватило)
                return (int)Math.Ceiling(materialWithDefect);
            }
            catch
            {
                return -1; // Любая ошибка – возвращаем -1
            }
        }
    }
}