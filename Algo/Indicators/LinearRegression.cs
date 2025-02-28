namespace StockSharp.Algo.Indicators
{
	using System.ComponentModel;

	using Ecng.Serialization;

	using StockSharp.Localization;

	using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

	/// <summary>
	/// The full class of linear regression, calculates LinearReg, LinearRegSlope, RSquared and StandardError at the same time.
	/// </summary>
	[DisplayName("LinearRegression")]
	[DescriptionLoc(LocalizedStrings.Str735Key)]
	public class LinearRegression : BaseComplexIndicator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LinearRegression"/>.
		/// </summary>
		public LinearRegression()
			: this(new LinearReg(), new RSquared(), new LinearRegSlope(), new StandardError())
		{
			Length = 11;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinearRegression"/>.
		/// </summary>
		/// <param name="linearReg">Linear regression.</param>
		/// <param name="rSquared">Regression R-squared.</param>
		/// <param name="regSlope">Coefficient with independent variable, slope of a straight line.</param>
		/// <param name="standardError">Standard error.</param>
		public LinearRegression(LinearReg linearReg, RSquared rSquared, LinearRegSlope regSlope, StandardError standardError)
			: base(linearReg, rSquared, regSlope, standardError)
		{
			LinearReg = linearReg;
			RSquared = rSquared;
			LinearRegSlope = regSlope;
			StandardError = standardError;

			Mode = ComplexIndicatorModes.Parallel;
		}

		/// <summary>
		/// Period length.
		/// </summary>
		[DisplayNameLoc(LocalizedStrings.Str736Key)]
		[DescriptionLoc(LocalizedStrings.Str737Key)]
		[CategoryLoc(LocalizedStrings.GeneralKey)]
		public int Length
		{
			get { return LinearReg.Length; }
			set
			{
				LinearReg.Length = RSquared.Length = LinearRegSlope.Length = StandardError.Length = value;
				Reset();
			}
		}

		/// <summary>
		/// Linear regression.
		/// </summary>
		[ExpandableObject]
		[DisplayName("LinearReg")]
		[DescriptionLoc(LocalizedStrings.Str738Key)]
		[CategoryLoc(LocalizedStrings.GeneralKey)]
		public LinearReg LinearReg { get; private set; }

		/// <summary>
		/// Regression R-squared.
		/// </summary>
		[ExpandableObject]
		[DisplayName("RSquared")]
		[DescriptionLoc(LocalizedStrings.Str739Key)]
		[CategoryLoc(LocalizedStrings.GeneralKey)]
		public RSquared RSquared { get; private set; }

		/// <summary>
		/// Standard error.
		/// </summary>
		[ExpandableObject]
		[DisplayName("StdErr")]
		[DescriptionLoc(LocalizedStrings.Str740Key)]
		[CategoryLoc(LocalizedStrings.GeneralKey)]
		public StandardError StandardError { get; private set; }

		/// <summary>
		/// Coefficient with independent variable, slope of a straight line.
		/// </summary>
		[ExpandableObject]
		[DisplayName("LinearRegSlope")]
		[DescriptionLoc(LocalizedStrings.Str741Key)]
		[CategoryLoc(LocalizedStrings.GeneralKey)]
		public LinearRegSlope LinearRegSlope { get; private set; }

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