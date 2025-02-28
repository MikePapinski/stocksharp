namespace StockSharp.Algo.Indicators
{
	using System;
	using System.ComponentModel;

	using Ecng.Common;
	using Ecng.Serialization;

	using StockSharp.Localization;

	using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

	/// <summary>
	/// Welles Wilder Directional Movement Index.
	/// </summary>
	[DisplayName("DX")]
	[DescriptionLoc(LocalizedStrings.Str762Key)]
	public class DirectionalIndex : BaseComplexIndicator
	{
		private sealed class DxValue : ComplexIndicatorValue
		{
			private decimal _value;

			public DxValue(IIndicator indicator)
				: base(indicator)
			{
			}

			public override IIndicatorValue SetValue<T>(IIndicator indicator, T value)
			{
				IsEmpty = false;
				_value = value.To<decimal>();
				return new DecimalIndicatorValue(indicator, _value);
			}

			public override T GetValue<T>()
			{
				return _value.To<T>();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectionalIndex"/>.
		/// </summary>
		public DirectionalIndex()
		{
			InnerIndicators.Add(Plus = new DiPlus());
			InnerIndicators.Add(Minus = new DiMinus());
		}

		/// <summary>
		/// Period length.
		/// </summary>
		[DisplayNameLoc(LocalizedStrings.Str736Key)]
		[DescriptionLoc(LocalizedStrings.Str737Key)]
		[CategoryLoc(LocalizedStrings.GeneralKey)]
		public virtual int Length
		{
			get { return Plus.Length; }
			set
			{
				Plus.Length = Minus.Length = value;
				Reset();
			}
		}

		/// <summary>
		/// DI+.
		/// </summary>
		[ExpandableObject]
		[DisplayName("DI+")]
		[Description("DI+.")]
		[CategoryLoc(LocalizedStrings.GeneralKey)]
		public DiPlus Plus { get; private set; }

		/// <summary>
		/// DI-.
		/// </summary>
		[ExpandableObject]
		[DisplayName("DI-")]
		[Description("DI-.")]
		[CategoryLoc(LocalizedStrings.GeneralKey)]
		public DiMinus Minus { get; private set; }

		/// <summary>
		/// To handle the input value.
		/// </summary>
		/// <param name="input">The input value.</param>
		/// <returns>The resulting value.</returns>
		protected override IIndicatorValue OnProcess(IIndicatorValue input)
		{
			var value = new DxValue(this) { IsFinal = input.IsFinal };

			var plusValue = Plus.Process(input);
			var minusValue = Minus.Process(input);

			value.InnerValues.Add(Plus, plusValue);
			value.InnerValues.Add(Minus, minusValue);

			if (plusValue.IsEmpty || minusValue.IsEmpty)
				return value;

			var plus = plusValue.GetValue<decimal>();
			var minus = minusValue.GetValue<decimal>();

			var diSum = plus + minus;
			var diDiff = Math.Abs(plus - minus);

			return value.SetValue(this, diSum != 0m ? (100 * diDiff / diSum) : 0m);
		}

		/// <summary>
		/// Load settings.
		/// </summary>
		/// <param name="settings">Settings storage.</param>
		public override void Load(SettingsStorage settings)
		{
			base.Load(settings);
			Length = settings.GetValue<int>("Length");
		}

		/// <summary>
		/// Save settings.
		/// </summary>
		/// <param name="settings">Settings storage.</param>
		public override void Save(SettingsStorage settings)
		{
			base.Save(settings);
			settings.SetValue("Length", Length);
		}
	}
}