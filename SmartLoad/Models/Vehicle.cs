using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmartLoad.Models
{
    public class Vehicle
    {
        #region Общее
        public int Id { get; set; }

        [Required(ErrorMessage = "Регистрационный номер обязателен")]
        [StringLength(20, ErrorMessage = "Регистрационный номер не должен превышать 20 символов")]
        [Display(Name = "Регистрационный номер")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Примечания не должны превышать 500 символов")]
        [Display(Name = "Примечание")]
        public string Notes { get; set; }
        #endregion

        #region Параметры тягача
        // Параметры тягача
        [Required(ErrorMessage = "Модель тягача обязательна")]
        [StringLength(100, ErrorMessage = "Модель тягача не должна превышать 100 символов")]
        [Display(Name = "Модель тягача")]
        public string TractorModel { get; set; }

        [Required(ErrorMessage = "Собственный вес тягача обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Собственный вес тягача должен быть положительным числом")]
        [Display(Name = "Собственный вес тягача (кг)")]
        public float TractorEmptyWeight { get; set; }

        [Required(ErrorMessage = "Количество осей тягача обязательно")]
        [Range(1, 3, ErrorMessage = "Количество осей тягача должно быть от 1 до 3")]
        [Display(Name = "Количество осей тягача")]
        public int TractorAxleCount { get; set; }

        [Required(ErrorMessage = "Тип передней оси тягача обязателен")]
        [StringLength(20, ErrorMessage = "Тип передней оси тягача не должен превышать 20 символов")]
        [Display(Name = "Тип передней оси тягача (односкатная/двускатная)")]
        public string TractorFrontAxleType { get; set; }

        [Required(ErrorMessage = "Максимальная нагрузка на переднюю ось тягача обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальная нагрузка на переднюю ось тягача должна быть положительным числом")]
        [Display(Name = "Макс. нагрузка на переднюю ось тягача (кг)")]
        public float TractorMaxFrontAxleLoad { get; set; }

        [Required(ErrorMessage = "Нагрузка на переднюю ось тягача в пустом состоянии обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Нагрузка на переднюю ось тягача в пустом состоянии должна быть положительным числом")]
        [Display(Name = "Нагрузка на переднюю ось тягача в пустом состоянии (кг)")]
        public float TractorEmptyFrontAxleLoad { get; set; }

        [Required(ErrorMessage = "Тип задней оси тягача обязателен")]
        [StringLength(20, ErrorMessage = "Тип задней оси тягача не должен превышать 20 символов")]
        [Display(Name = "Тип задней оси тягача (односкатная/двускатная)")]
        public string TractorRearAxleType { get; set; }

        [Required(ErrorMessage = "Максимальная нагрузка на заднюю ось тягача обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальная нагрузка на заднюю ось тягача должна быть положительным числом")]
        [Display(Name = "Макс. нагрузка на заднюю ось тягача (кг)")]
        public float TractorMaxRearAxleLoad { get; set; }

        [Required(ErrorMessage = "Нагрузка на заднюю ось тягача в пустом состоянии обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Нагрузка на заднюю ось тягача в пустом состоянии должна быть положительным числом")]
        [Display(Name = "Нагрузка на заднюю ось тягача в пустом состоянии (кг)")]
        public float TractorEmptyRearAxleLoad { get; set; }

        [Required(ErrorMessage = "Колесная база тягача обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Колесная база тягача должна быть положительным числом")]
        [Display(Name = "Колесная база тягача (мм)")]
        public float TractorWheelBase { get; set; }

        [Required(ErrorMessage = "Расстояние от задней оси тягача до шкворня обязательно")]
        [Range(0, float.MaxValue, ErrorMessage = "Расстояние от задней оси тягача до шкворня должно быть положительным числом")]
        [Display(Name = "Расстояние от задней оси тягача до шкворня (мм)")]
        public float TractorRearAxleToKingpin { get; set; }

        [Required(ErrorMessage = "Максимальная грузоподъемность тягача обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальная грузоподъемность тягача должна быть положительным числом")]
        [Display(Name = "Макс. грузоподъемность тягача (кг)")]
        public float TractorMaxLoadCapacity { get; set; }
        #endregion

        #region Параметры полуприцепа
        // Параметры полуприцепа
        [Required(ErrorMessage = "Модель полуприцепа обязательна")]
        [StringLength(100, ErrorMessage = "Модель полуприцепа не должна превышать 100 символов")]
        [Display(Name = "Модель полуприцепа")]
        public string TrailerModel { get; set; }

        [Required(ErrorMessage = "Собственный вес полуприцепа обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Собственный вес полуприцепа должен быть положительным числом")]
        [Display(Name = "Собственный вес полуприцепа (кг)")]
        public float TrailerEmptyWeight { get; set; }

        [Required(ErrorMessage = "Длина полуприцепа обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Длина полуприцепа должна быть положительным числом")]
        [Display(Name = "Длина полуприцепа (мм)")]
        public float TrailerLength { get; set; }

        [Required(ErrorMessage = "Ширина полуприцепа обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Ширина полуприцепа должна быть положительным числом")]
        [Display(Name = "Ширина полуприцепа (мм)")]
        public float TrailerWidth { get; set; }

        [Required(ErrorMessage = "Высота полуприцепа обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Высота полуприцепа должна быть положительным числом")]
        [Display(Name = "Высота полуприцепа (мм)")]
        public float TrailerHeight { get; set; }

        [Required(ErrorMessage = "Количество осей полуприцепа обязательно")]
        [Range(1, 5, ErrorMessage = "Количество осей полуприцепа должно быть от 1 до 5")]
        [Display(Name = "Количество осей полуприцепа")]
        public int TrailerAxleCount { get; set; }

        [Required(ErrorMessage = "Тип осей полуприцепа обязателен")]
        [StringLength(20, ErrorMessage = "Тип осей полуприцепа не должен превышать 20 символов")]
        [Display(Name = "Тип осей полуприцепа (односкатные/двускатные)")]
        public string TrailerAxleType { get; set; }

        [Required(ErrorMessage = "Максимальная нагрузка на ось полуприцепа обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальная нагрузка на ось полуприцепа должна быть положительным числом")]
        [Display(Name = "Макс. нагрузка на ось полуприцепа (кг)")]
        public float TrailerMaxAxleLoad { get; set; }

        [Required(ErrorMessage = "Расстояние от шкворня до центра тележки осей полуприцепа обязательно")]
        [Range(0, float.MaxValue, ErrorMessage = "Расстояние от шкворня до центра тележки осей полуприцепа должно быть положительным числом")]
        [Display(Name = "Расстояние от шкворня до центра тележки осей полуприцепа (мм)")]
        public float TrailerKingpinToAxle { get; set; }

        [Required(ErrorMessage = "Расстояние между крайними осями полуприцепа обязательно")]
        [Range(0, float.MaxValue, ErrorMessage = "Расстояние между крайними осями полуприцепа должно быть положительным числом")]
        [Display(Name = "Расстояние между крайними осями полуприцепа (мм)")]
        public float TrailerAxleSpread { get; set; }

        [Required(ErrorMessage = "Максимальная грузоподъемность полуприцепа обязательна")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальная грузоподъемность полуприцепа должна быть положительным числом")]
        [Display(Name = "Макс. грузоподъемность полуприцепа (кг)")]
        public float TrailerMaxLoadCapacity { get; set; }

        [Required(ErrorMessage = "Максимальный объем полуприцепа обязателен")]
        [Range(0, float.MaxValue, ErrorMessage = "Максимальный объем полуприцепа должен быть положительным числом")]
        [Display(Name = "Макс. объем полуприцепа (м³)")]
        public float TrailerMaxVolumeCapacity { get; set; }

        #endregion

        // Навигационные свойства
        [ValidateNever]
        public List<LoadingScheme> LoadingSchemes { get; set; } = new List<LoadingScheme>();

        // Вычисляемые свойства
        [NotMapped]
        public float TotalEmptyWeight => TractorEmptyWeight + TrailerEmptyWeight;

        [NotMapped]
        public float TotalMaxLoadCapacity => TractorMaxLoadCapacity + TrailerMaxLoadCapacity;

        // Базовые методы для работы с данными транспортного средства
        // Метод для расчета нагрузки на оси при заданном весе груза и положении центра тяжести
        // Метод для расчета нагрузки на оси при заданном весе груза и положении центра тяжести
        public Dictionary<string, float> CalculateAxleLoads(float cargoWeight, float cargoPositionFromKingpin)
        {
            var axleLoads = new Dictionary<string, float>();

            // Расчет нагрузки на шкворень от полуприцепа в пустом состоянии
            float emptyKingpinLoad = TrailerEmptyWeight * (TrailerKingpinToAxle / TrailerLength);

            // Расчет нагрузки на оси полуприцепа в пустом состоянии
            float emptyTrailerAxleLoad = TrailerEmptyWeight - emptyKingpinLoad;

            // Расчет дополнительной нагрузки на шкворень от груза
            float cargoKingpinLoad = 0;
            if (cargoWeight > 0)
            {
                cargoKingpinLoad = cargoWeight * ((TrailerLength - cargoPositionFromKingpin) / TrailerLength);
                if (cargoKingpinLoad < 0) cargoKingpinLoad = 0; // Если груз за осями полуприцепа
            }

            // Расчет дополнительной нагрузки на оси полуприцепа от груза
            float cargoTrailerAxleLoad = cargoWeight - cargoKingpinLoad;

            // Общая нагрузка на шкворень
            float totalKingpinLoad = emptyKingpinLoad + cargoKingpinLoad;

            // Распределение нагрузки от шкворня на оси тягача
            float rearAxleDistribution = TractorRearAxleToKingpin / TractorWheelBase;
            float frontAxleDistribution = 1 - rearAxleDistribution;

            // Дополнительная нагрузка на оси тягача от груза через шкворень
            float additionalRearAxleLoad = totalKingpinLoad * rearAxleDistribution;
            float additionalFrontAxleLoad = totalKingpinLoad * frontAxleDistribution;

            // Итоговая нагрузка на оси тягача
            float tractorRearAxleLoad = TractorEmptyRearAxleLoad + additionalFrontAxleLoad;
            float tractorFrontAxleLoad = TractorEmptyFrontAxleLoad + additionalRearAxleLoad;

            // Общая нагрузка на оси полуприцепа
            float trailerAxleLoad = emptyTrailerAxleLoad + cargoTrailerAxleLoad;

            // Заполняем словарь с результатами
            axleLoads.Add("TractorFrontAxle", tractorFrontAxleLoad);
            axleLoads.Add("TractorRearAxle", tractorRearAxleLoad);
            axleLoads.Add("TrailerAxles", trailerAxleLoad);
            axleLoads.Add("KingpinLoad", totalKingpinLoad);

            return axleLoads;
        }


        // Метод для проверки допустимости нагрузок на оси
        public bool ValidateAxleLoads(Dictionary<string, float> axleLoads)
        {
            // Проверка нагрузки на переднюю ось тягача
            if (axleLoads["TractorFrontAxle"] > TractorMaxFrontAxleLoad)
            {
                return false;
            }

            // Проверка нагрузки на заднюю ось тягача
            if (axleLoads["TractorRearAxle"] > TractorMaxRearAxleLoad)
            {
                return false;
            }

            // Проверка нагрузки на оси полуприцепа
            // Делим общую нагрузку на количество осей полуприцепа
            float loadPerTrailerAxle = axleLoads["TrailerAxles"] / TrailerAxleCount;
            if (loadPerTrailerAxle > TrailerMaxAxleLoad)
            {
                return false;
            }

            // Проверка нагрузки на шкворень (обычно не должна превышать 12-15 тонн)
            //if (axleLoads["KingpinLoad"] > 15000) // Примерное ограничение в 15 тонн
            //{
            //    return false;
            //}

            return true;
        }

        // Метод для проверки, помещается ли груз в полуприцеп по объему
        public bool ValidateVolumeCapacity(float cargoVolume)
        {
            return cargoVolume <= TrailerMaxVolumeCapacity;
        }

        // Новый метод в классе Vehicle
        public Dictionary<string, float> EstimateAxleLoads(float additionalWeight, float positionFromKingpin, Dictionary<string, float> currentLoads)
        {
            var newLoads = new Dictionary<string, float>();

            // Упрощённый расчёт изменения нагрузок
            float kingpinLoadDelta = additionalWeight * (TrailerLength - positionFromKingpin) / TrailerLength;
            float trailerAxleDelta = additionalWeight - kingpinLoadDelta;

            newLoads["TractorFrontAxle"] = currentLoads["TractorFrontAxle"] +
                kingpinLoadDelta * (TractorRearAxleToKingpin / TractorWheelBase);

            newLoads["TractorRearAxle"] = currentLoads["TractorRearAxle"] +
                kingpinLoadDelta * (1 - TractorRearAxleToKingpin / TractorWheelBase);

            newLoads["TrailerAxles"] = currentLoads["TrailerAxles"] + trailerAxleDelta;

            return newLoads;
        }

        // Метод для проверки, помещается ли груз в полуприцеп по весу
        public bool ValidateWeightCapacity(float cargoWeight)
        {
            return cargoWeight <= TrailerMaxLoadCapacity;
        }
    }
}

