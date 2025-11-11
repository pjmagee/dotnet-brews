using Microsoft.Extensions.Logging;

namespace Brew.Features.Builders.FluentBuilder;

/// <summary>
/// Demonstrates a FLUENT (step-wise) builder enforcing construction order.
/// Example domain: Building a Report with required Title, then one or more Sections, then optional Footer.
/// The interfaces restrict calling order: Title -> Sections -> Footer -> Build.
/// </summary>
public interface IReportTitleStep { IReportContentStep WithTitle(string title); }
public interface IReportContentStep { IReportContentStep AddSection(string heading, string body); IReportFooterStep WithFooter(string footerText); IReportBuildStep WithoutFooter(); }
public interface IReportFooterStep { IReportBuildStep WithFooter(string footerText); }
public interface IReportBuildStep { Report Build(); }

public sealed class Report
{
    public string Title { get; set; } = string.Empty;
    public List<(string Heading,string Body)> Sections { get; } = new();
    public string? Footer { get; set; }

    public override string ToString() => $"Report: {Title} | Sections: {Sections.Count} | Footer: {(Footer is null ? "None" : "Present" )}";
}

public class FluentBuilder : IReportTitleStep, IReportContentStep, IReportFooterStep, IReportBuildStep
{
    private readonly ILogger<FluentBuilder> _logger;
    private readonly Report _report = new() { Title = "" };
    private bool _titleSet;

    public FluentBuilder(ILogger<FluentBuilder> logger) => _logger = logger;

    public IReportContentStep WithTitle(string title)
    {
        _report.Title = title;
        _titleSet = true;
        _logger.LogInformation("[Builder] Title set: {Title}", title);
        return this;
    }

    public IReportContentStep AddSection(string heading, string body)
    {
        if(!_titleSet)
            throw new InvalidOperationException("Title must be set before adding sections.");
        _report.Sections.Add((heading, body));
        _logger.LogInformation("[Builder] Added section: {Heading} (length {Length} chars)", heading, body.Length);
        return this;
    }

    public IReportFooterStep WithFooter(string footerText)
    {
        if(!_titleSet)
            throw new InvalidOperationException("Title must be set before footer.");
        _report.Footer = footerText;
        _logger.LogInformation("[Builder] (Interim) Footer prepared (will finalize on Build)");
        return this;
    }

    IReportBuildStep IReportFooterStep.WithFooter(string footerText)
    {
        _report.Footer = footerText;
        _logger.LogInformation("[Builder] Footer set: {Footer}", footerText);
        return this;
    }

    public IReportBuildStep WithoutFooter()
    {
        _logger.LogInformation("[Builder] Proceeding without footer.");
        return this;
    }

    public Report Build()
    {
        if(!_titleSet) throw new InvalidOperationException("Cannot build report; title missing.");
        if(_report.Sections.Count == 0) _logger.LogWarning("[Builder] Building report with NO sections.");
        _logger.LogInformation("[Builder] Report build complete. Title='{Title}', Sections={Count}, Footer={HasFooter}", _report.Title, _report.Sections.Count, _report.Footer is not null);
        return _report;
    }
}