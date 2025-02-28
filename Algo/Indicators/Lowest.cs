namespace StockSharp.Algo.Indicators
{
	using System;
	using System.ComponentModel;
	using System.Linq;

	using StockSharp.Localization;

	/// <summary>
	/// Minimum value for a period.
	/// </summary>
	[DisplayName("Lowest")]
	[DescriptionLoc(LocalizedStrings.Str743Key)]
	public class Lowest : LengthIndicator<decimal>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Lowest"/>.
		/// </summary>
		public Lowest()
		{
			Length = 5;
		}

		/// <summary>
		/// To handle the input value.
		/// </summary>
		/// <param name="input">The input value.</param>
		/// <returns>The resulting value.</returns>
		protected override IIndicatorValue OnProcess(IIndicatorValue input)
		{
			var newValue = input.GetValue<decimal>();

			var lastValue = Buffer.Count == 0 ? newValue : this.GetCurrentValue();

			// добавляем новое начало
			if (input.IsFinal)
				Buffer.Add(newValue);

			if (newValue < lastValue)
			{
				// Новое значение и есть экстремум 
				lastValue = newValue;
			}

			if (Buffer.Count > Length) // IsFormed не использовать, т.к. сначала добавляется и >= не подходит
			{
				var first = Buffer[0];

				// удаляем хвостовое значение
				if (input.IsFinal)
					Buffer.RemoveAt(0);

				if (first == lastValue && lastValue != newValue) // удаляется экстремум, для поиска нового значения необходим проход по всему буфферу
				{
					// ищем новый экстремум
					lastValue = Buffer.Aggregate(newValue, (current, t) => Math.Min(t, current));
				}
			}

			return new DecimalIndicatorValue(this, lastValue);
		}
	}
}