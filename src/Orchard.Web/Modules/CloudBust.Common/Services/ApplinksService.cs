using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Caching;
using Orchard.Data;
using Orchard.Localization;
using CloudBust.Common.Models;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Services {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class ApplinksService : IApplinksService {

		private const string DefaultApplinksText = @"{}";

		private readonly IRepository<ApplinksFileRecord> _repository;
		private readonly ISignals _signals;

		public ApplinksService(IRepository<ApplinksFileRecord> repository, ISignals signals) {
			_repository = repository;
			_signals = signals;
			T = NullLocalizer.Instance;
		}

		public Localizer T { get; set; }

		public ApplinksFileRecord Get() {
			var applinksFileRecord = _repository.Table.FirstOrDefault();
			if (applinksFileRecord == null) {
				applinksFileRecord = new ApplinksFileRecord() {
					FileContent = DefaultApplinksText
				};
				_repository.Create(applinksFileRecord);
			}
			return applinksFileRecord;
		}

		public Tuple<bool, IEnumerable<string>> Save(string text) {
			var applinksFileRecord = Get();
			applinksFileRecord.FileContent = text;
			_signals.Trigger("ApplinksFile.SettingsChanged");
			var validationResult = Validate(text);
			return validationResult;
		}

		private Tuple<bool, IEnumerable<string>> Validate(string text) {
			using (var validatingReader = new ApplinksFileValidatingReader(text)) {
				validatingReader.ValidateToEnd();
				return new Tuple<bool, IEnumerable<string>>(validatingReader.IsValid, validatingReader.Errors.Select(error =>
					T("Line {0}: {1}, {2}", error.LineNumber, error.BadContent, error.Error).ToString()
				));
			}
		}
	}
}