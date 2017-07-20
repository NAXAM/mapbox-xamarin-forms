namespace Naxam.Controls.Forms
{
	public class Annotation: Xamarin.Forms.BindableObject
    {
		public Annotation()
		{
		}

		public string Id
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string SubTitle
		{
			get;
			set;
		}

        public object Native
        {
            get;
            set;
        }
	}
}
