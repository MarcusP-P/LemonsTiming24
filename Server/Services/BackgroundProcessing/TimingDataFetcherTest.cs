
using LemonsTiming24.Server.Infrastructure;
using LemonsTiming24.Server.Model.RawTiming;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LemonsTiming24.Server.Services.BackgroundProcessing;
public class TimingDataFetcherTest : ITimingDataFetcher
{
    private readonly IOptions<TimingConfiguration> timingConfiguration;

    public TimingDataFetcherTest(IOptions<TimingConfiguration> timingConfiguration)
    {
        ArgumentNullException.ThrowIfNull(timingConfiguration, nameof(timingConfiguration));

        this.timingConfiguration = timingConfiguration;
    }

    public async Task DoWork(CancellationToken cancellationToken)
    {
        var directoryInfo = new DirectoryInfo(this.timingConfiguration.Value?.SavedMessagesPath ?? "");

        var fileList = directoryInfo.GetFiles()
            .Where(x =>
                x.Name.StartsWith("best_sectors-", StringComparison.InvariantCulture)
                || x.Name.StartsWith("entries-", StringComparison.InvariantCulture)
                || x.Name.StartsWith("flags-", StringComparison.InvariantCulture)
                || x.Name.StartsWith("laps-", StringComparison.InvariantCulture)
                || x.Name.StartsWith("params-", StringComparison.InvariantCulture)
                || x.Name.StartsWith("race-", StringComparison.InvariantCulture)
                || x.Name.StartsWith("race_light-", StringComparison.InvariantCulture)
                || x.Name.StartsWith("stints-", StringComparison.InvariantCulture)
                )
            .OrderBy(x => x.Name.Remove(0, x.Name.IndexOf("-", StringComparison.InvariantCulture) + 1))
            .ToArray();

        var total = fileList.Length;
        var current = 1;
        foreach (var file in fileList)
        {
            var fileValue = await File.ReadAllTextAsync(file.FullName, cancellationToken);

            var foo = JsonDocument.Parse(fileValue);

            System.Diagnostics.Debug.Print($"Extracting {current}/{total} {(float)current / total:P3} {file.Name.Remove(0, file.Name.IndexOf("-", StringComparison.InvariantCulture) + 1)} {file.Name}");
            if (file.Name.StartsWith("race-", StringComparison.InvariantCulture))
            {
                foreach (var foo2 in foo.RootElement.EnumerateArray())
                {
                    if (foo2.ValueKind == JsonValueKind.Object)
                    {
                        var foo3 = JsonSerializer.Deserialize<Race>(foo2.GetRawText());
                        if (foo3?.ExtensionData != null)
                        {
                            System.Diagnostics.Debugger.Break();
                        }

                        if (foo3?.Paramaters?.ExtensionData != null)
                        {
                            System.Diagnostics.Debugger.Break();
                        }

                        if (foo3?.ProgressFlagState != null)
                        {
                            var foo4 = foo3.ProgressFlagState
                                .Where(x => x?.ExtensionData != null)
                                .ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }

                        if (foo3?.Entries != null)
                        {
                            var foo4 = foo3.Entries.Where(x => x?.ExtensionData != null).ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo5 = foo3.Entries.Where(x => x?.Drivers != null)
                                .SelectMany(x => x.Drivers!)
                                .Where(x => x.ExtensionData != null)
                                .ToList();
                            if (foo5.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }

                        if (foo3?.BestSectors != null)
                        {
                            var foo4 = foo3.BestSectors
                                .Where(x => x?.ExtensionData != null)
                                .ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }

                        if (foo3?.BestTimesByCategory != null)
                        {
                            var foo4 = foo3.BestTimesByCategory
                                .Where(x => x?.ExtensionData != null)
                                .ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }
                    }
                }
            }

            else if (file.Name.StartsWith("best_sectors-", StringComparison.InvariantCulture))
            {
                foreach (var foo2 in foo.RootElement.EnumerateArray())
                {
                    if (foo2.ValueKind == JsonValueKind.Array)
                    {
                        var foo3 = JsonSerializer.Deserialize<BestSector[]>(foo2.GetRawText());
                        if (foo3?.Any() ?? false)
                        {
                            var foo4 = foo3.Where(x => x?.ExtensionData != null).ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }
                    }
                }
            }

            else if (file.Name.StartsWith("entries-", StringComparison.InvariantCulture))
            {
                foreach (var foo2 in foo.RootElement.EnumerateArray())
                {
                    if (foo2.ValueKind == JsonValueKind.Array)
                    {
                        var foo3 = JsonSerializer.Deserialize<Entry[]>(foo2.GetRawText());
                        if (foo3?.Any() ?? false)
                        {
                            var foo4 = foo3.Where(x => x?.ExtensionData != null).ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo5 = foo3.Where(x => x?.Drivers != null)
                                .SelectMany(x => x.Drivers!)
                                .Where(x => x.ExtensionData != null)
                                .ToList();
                            if (foo5.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }
                    }
                }
            }

            else if (file.Name.StartsWith("stints-", StringComparison.InvariantCulture))
            {
                foreach (var foo2 in foo.RootElement.EnumerateArray())
                {
                    if (foo2.ValueKind == JsonValueKind.Array)
                    {
                        var foo3 = JsonSerializer.Deserialize<Stints[]>(foo2.GetRawText());
                        if (foo3?.Any() ?? false)
                        {
                            var foo4 = foo3.Where(x => x?.ExtensionData != null).ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo5 = foo3.Where(x => x?.IndividualStints != null)
                                .SelectMany(x => x.IndividualStints?.Values!)
                                .Where(x => x?.ExtensionData != null)
                                .ToList();
                            if (foo5.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }
                    }
                }
            }

            else if (file.Name.StartsWith("laps-", StringComparison.InvariantCulture))
            {
                foreach (var foo2 in foo.RootElement.EnumerateArray())
                {
                    if (foo2.ValueKind == JsonValueKind.Array)
                    {
                        var foo3 = JsonSerializer.Deserialize<Laps[]>(foo2.GetRawText());
                        if (foo3?.Any() ?? false)
                        {
                            var foo4 = foo3.Where(x => x?.ExtensionData != null).ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo5 = foo3.Where(x => x?.CarLaps != null)
                                .SelectMany(x => x.CarLaps!.Values);

                            var foo6 = foo5.Where(x => x.ExtensionData != null);
                            if (foo6.Any())
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo7 = foo5.Where(x => x.LoopSectors != null)
                                .SelectMany(x => x.LoopSectors!.Values)
                                .Where(x => x?.ExtensionData != null)
                                .ToList();
                            if (foo7.Any())
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo8 = foo5
                                .Where(x => x.PitOut?.ExtensionData != null)
                                .ToList();
                            if (foo8.Any())
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo9 = foo5
                                .Where(x => x.PitIn?.ExtensionData != null)
                                .ToList();
                            if (foo9.Any())
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo10 = foo5.Where(x => x.Sections != null)
                                .SelectMany(x => x.Sections!.Values)
                                .Where(x => x?.ExtensionData != null)
                                .ToList();
                            if (foo10.Any())
                            {
                                System.Diagnostics.Debugger.Break();
                            }

                            var foo11 = foo5.Where(x => x.Sectors != null)
                                .SelectMany(x => x.Sectors!.Values)
                                .Where(x => x?.ExtensionData != null)
                                .ToList();
                            if (foo11.Any())
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }
                    }
                }
            }

            else if (file.Name.StartsWith("flags-", StringComparison.InvariantCulture))
            {
                foreach (var foo2 in foo.RootElement.EnumerateArray())
                {
                    if (foo2.ValueKind == JsonValueKind.Array)
                    {
                        var foo3 = JsonSerializer.Deserialize<Flags[]>(foo2.GetRawText());
                        if (foo3?.Any() ?? false)
                        {
                            var foo4 = foo3.Where(x => x?.ExtensionData != null).ToList();
                            if (foo4.Count != 0)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        }
                    }
                }
            }

            else if (file.Name.StartsWith("params-", StringComparison.InvariantCulture)
                || file.Name.StartsWith("race_light-", StringComparison.InvariantCulture))
            {
                foreach (var foo2 in foo.RootElement.EnumerateArray())
                {
                    if (foo2.ValueKind == JsonValueKind.Object)
                    {
                        var foo3 = JsonSerializer.Deserialize<Paramaters>(foo2.GetRawText());
                        if (foo3?.ExtensionData != null)
                        {
                            System.Diagnostics.Debugger.Break();
                        }
                    }
                }
            }

            current++;
        }
    }
}
