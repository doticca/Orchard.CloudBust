using System;
using System.Collections.Generic;
using System.IO;
using Orchard.Localization;
using System.Text.RegularExpressions;
using Orchard.Environment.Extensions;

namespace CloudBust.Common.Services {

    [OrchardFeature("CloudBust.Common.SEO")]
    public class ApplinksFileValidatingReader : IDisposable {
		private const string SyntaxNotUnderstoodError = "Syntax not understood";
		private const string NoUserAgentSpecifiedError = "No user-agent specified";
		private const string NewLineRequiredError = "An empty line is required between user agent blocks";
		private const string InvalidSitemapUrl = "The Sitemap URL provided is invalid or is not absolute";
		private const string InvalidCrawlDelayValue = "The Crawl-delay value must be a non-negative whole number";

		private List<ApplinksFileValidationError> _errors;
		private StringReader _reader;
		private int _currentLineNumber;
		private string _currentUserAgent = null;

		public ApplinksFileValidatingReader(string applinksFileText) {
			_errors = new List<ApplinksFileValidationError>();
			_reader = new StringReader(applinksFileText);
			_currentLineNumber = 0;
			T = NullLocalizer.Instance;
		}

		public Localizer T { get; set; }

		public bool ValidateNext() {
			string line = ReadLine();
			if (line == null)
				return false;
			Validate(line);
			_currentLineNumber++;
			return true;
		}

		public void ValidateToEnd() {
			string line = ReadLine();
			while (line != null) {
				Validate(line);
				line = ReadLine();
			}
		}

		private string ReadLine() {
			string line = null;
			try {
				line = _reader.ReadLine();
				return line;
			}
			finally {
				if (line != null)
					_currentLineNumber++;
			}
		}

		private void Validate(string line) {
			// Allow blank lines
			if (string.IsNullOrWhiteSpace(line)) {
				// If the line is blank, then that means we should get a new user agent directive on the following line or EOF
				_currentUserAgent = null;
				return;
			}
			// Allow comments, robots.txt comments start with # (pound), in this case, the entire line is ignored
			if (line.StartsWith("#"))
				return;
		}

		public List<ApplinksFileValidationError> Errors { get { return _errors; } }
		public bool IsValid { get { return _errors.Count == 0; } }
		public int CurrentLineNumber { get { return _currentLineNumber; } }
		public string CurrentUserAgent { get { return _currentUserAgent; } }

		#region IDisposable Members

		public void Dispose() {
			_reader.Dispose();
		}

		#endregion
	}

	public class ApplinksFileValidationError {
		public ApplinksFileValidationError(int lineNumber, string badContent, string error) {
			LineNumber = lineNumber;
			BadContent = badContent;
			Error = error;
		}
		public int LineNumber { get; private set; }
		public string BadContent { get; private set; }
		public string Error { get; private set; }
	}
}