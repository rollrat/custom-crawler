/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using MasterDevs.ChromeDevTools;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomCrawler.Analysis
{
    public class CallTreeManager
    {
        static Dictionary<string, (string, Location[])> break_points;
        static int bp_count;
        static int paused_count;
        static List<PausedEvent> paused = new List<PausedEvent>();
        static List<(ScriptParsedEvent, string)> anonymous_scripts = new List<(ScriptParsedEvent, string)>();
        public static async void Init(CustomCrawlerDynamics dyn)
        {
            var cc = dyn.scripts.ToList();
            break_points = new Dictionary<string, (string, Location[])>();

            await CustomCrawlerDynamics.ss.SendAsync<SetBreakpointsActiveCommand>();

            CustomCrawlerDynamics.ss.Subscribe<BreakpointResolvedEvent>(x =>
            {
                var y = x;
                bp_count++;
            });

            CustomCrawlerDynamics.ss.Subscribe<PausedEvent>(async x =>
            {
                paused.Add(x);
                paused_count++;
                await CustomCrawlerDynamics.ss.SendAsync<ResumeCommand>();
            });

            var tasks = new List<(int, GetPossibleBreakpointsCommandResponse)>();
            var total_tasks = 0;
            var succ_tasks = 0;
            var wtasks1 = new List<Task>();

            for (int i = 0; i < cc.Count; i++)
            {
                var x = i;
                wtasks1.Add(Task.Run(async () =>
                {
                    var y = x;
                    var ss = cc[y];
                    var res = await CustomCrawlerDynamics.ss.SendAsync(new GetPossibleBreakpointsCommand
                    {
                        Start = new Location { LineNumber = ss.StartLine, ColumnNumber = ss.StartColumn, ScriptId = ss.ScriptId },
                        End = new Location { LineNumber = ss.EndLine, ColumnNumber = ss.EndColumn, ScriptId = ss.ScriptId }
                    });

                    if (res.Result != null)
                    {
                        lock (tasks)
                            tasks.Add((y, res.Result));
                        Interlocked.Add(ref total_tasks, res.Result.Locations.Length);
                    }
                    else
                    {
                        var code = await CustomCrawlerDynamics.ss.SendAsync(new GetScriptSourceCommand
                        {
                            ScriptId = ss.ScriptId
                        });

                        if (code.Result != null)
                            lock (anonymous_scripts)
                                anonymous_scripts.Add((ss, code.Result.ScriptSource));
                        else
                            ;
                    }
                }));
            }

            await Task.WhenAll(wtasks1);

            ;

            var wtasks2 = new List<Task>();

            foreach (var yy in tasks)
            {
                var ss = cc[yy.Item1];
                foreach (var bb in yy.Item2.Locations)
                {
                    wtasks2.Add(Task.Run(async () =>
                    {
                        var rr = await CustomCrawlerDynamics.ss.SendAsync(new SetBreakpointByUrlCommand
                        {
                            LineNumber = bb.LineNumber,
                            ColumnNumber = bb.ColumnNumber,
                            Url = ss.Url,
                        });

                        lock (break_points)
                            break_points.Add(rr.Result.BreakpointId, (ss.Url, rr.Result.Locations));
                        Interlocked.Increment(ref succ_tasks);
                    }));
                }
            }

            await Task.WhenAll(wtasks2);

            ;
            //await CustomCrawlerDynamics.ss.SendAsync
        }
    }
}
