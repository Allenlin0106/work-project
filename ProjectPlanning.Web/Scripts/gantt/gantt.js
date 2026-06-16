(function (global) {
    'use strict';

    var SVG_NS = 'http://www.w3.org/2000/svg';

    function parseDate(s) {
        if (s instanceof Date) return s;
        if (typeof s === 'string' && s.indexOf('/Date(') === 0) {
            var ms = parseInt(s.substring(6), 10);
            return new Date(ms);
        }
        return new Date(s);
    }

    function dayDiff(a, b) {
        var ms = b.getTime() - a.getTime();
        return Math.round(ms / 86400000);
    }

    function startOfDay(d) {
        return new Date(d.getFullYear(), d.getMonth(), d.getDate());
    }

    function createSvg(name, attrs) {
        var el = document.createElementNS(SVG_NS, name);
        if (attrs) {
            for (var k in attrs) {
                if (Object.prototype.hasOwnProperty.call(attrs, k)) {
                    el.setAttribute(k, attrs[k]);
                }
            }
        }
        return el;
    }

    function render(container, data, opts) {
        opts = opts || {};
        var dayWidth = opts.dayWidth || 24;
        var rowHeight = opts.rowHeight || 28;
        var headerHeight = 32;
        var labelWidth = opts.labelWidth || 200;

        var tasks = (data.Tasks || data.tasks || []).slice().sort(function (a, b) {
            return (a.SortOrder || a.sortOrder || 0) - (b.SortOrder || b.sortOrder || 0);
        });
        var deps = data.Dependencies || data.dependencies || [];

        var projectStart = startOfDay(parseDate(data.StartDate || data.startDate));
        var projectEnd = startOfDay(parseDate(data.EndDate || data.endDate));

        tasks.forEach(function (t) {
            t._start = startOfDay(parseDate(t.StartDate || t.startDate));
            t._end = startOfDay(parseDate(t.EndDate || t.endDate));
            if (t._start < projectStart) projectStart = t._start;
            if (t._end > projectEnd) projectEnd = t._end;
        });

        var totalDays = Math.max(1, dayDiff(projectStart, projectEnd) + 1);
        var chartWidth = totalDays * dayWidth;
        var totalHeight = headerHeight + tasks.length * rowHeight + 12;
        var width = labelWidth + chartWidth + 8;

        var svg = createSvg('svg', {
            width: width,
            height: totalHeight,
            xmlns: SVG_NS,
            viewBox: '0 0 ' + width + ' ' + totalHeight
        });

        var defs = createSvg('defs');
        var marker = createSvg('marker', {
            id: 'gantt-arrow',
            viewBox: '0 0 10 10',
            refX: 9, refY: 5,
            markerWidth: 6, markerHeight: 6,
            orient: 'auto-start-reverse'
        });
        var arrowPath = createSvg('path', { d: 'M 0 0 L 10 5 L 0 10 z', fill: '#444' });
        marker.appendChild(arrowPath);
        defs.appendChild(marker);
        svg.appendChild(defs);

        var headerBg = createSvg('rect', {
            x: 0, y: 0, width: width, height: headerHeight, fill: '#eef2f8'
        });
        svg.appendChild(headerBg);

        var monthCursor = new Date(projectStart);
        for (var d = 0; d <= totalDays; d++) {
            var x = labelWidth + d * dayWidth;
            svg.appendChild(createSvg('line', {
                x1: x, y1: headerHeight, x2: x, y2: totalHeight,
                'class': 'gantt-grid'
            }));
            if (d < totalDays) {
                var date = new Date(projectStart.getTime() + d * 86400000);
                if (date.getDate() === 1 || d === 0) {
                    var label = (date.getMonth() + 1) + '/' + date.getDate();
                    var t = createSvg('text', {
                        x: x + 2, y: 12, 'class': 'gantt-axis'
                    });
                    t.textContent = date.getFullYear() + '-' + label;
                    svg.appendChild(t);
                }
                if (d % 7 === 0) {
                    var dow = createSvg('text', { x: x + 2, y: 26, 'class': 'gantt-axis' });
                    dow.textContent = (date.getMonth() + 1) + '/' + date.getDate();
                    svg.appendChild(dow);
                }
            }
        }

        tasks.forEach(function (t, i) {
            var y = headerHeight + i * rowHeight;
            var label = createSvg('text', {
                x: 6, y: y + rowHeight / 2 + 4, 'class': 'gantt-row-label'
            });
            label.textContent = t.Name || t.name;
            svg.appendChild(label);

            var sx = labelWidth + dayDiff(projectStart, t._start) * dayWidth;
            var ex = labelWidth + (dayDiff(projectStart, t._end) + 1) * dayWidth;
            var barWidth = Math.max(2, ex - sx);
            var barHeight = rowHeight - 8;

            svg.appendChild(createSvg('rect', {
                x: sx, y: y + 4, width: barWidth, height: barHeight,
                'class': 'gantt-bar', rx: 2, ry: 2
            }));

            var progress = (t.ProgressPercent || t.progressPercent || 0) / 100;
            if (progress > 0) {
                svg.appendChild(createSvg('rect', {
                    x: sx, y: y + 4, width: barWidth * progress, height: barHeight,
                    'class': 'gantt-progress', rx: 2, ry: 2
                }));
            }

            var pctText = createSvg('text', {
                x: sx + 4, y: y + rowHeight / 2 + 4, 'class': 'gantt-text'
            });
            pctText.textContent = (t.ProgressPercent || t.progressPercent || 0) + '%';
            svg.appendChild(pctText);

            t._barX1 = sx;
            t._barX2 = sx + barWidth;
            t._barY = y + rowHeight / 2;
        });

        var taskById = {};
        tasks.forEach(function (t) { taskById[t.Id || t.id] = t; });

        deps.forEach(function (dep) {
            var pre = taskById[dep.PredecessorTaskId || dep.predecessorTaskId];
            var suc = taskById[dep.SuccessorTaskId || dep.successorTaskId];
            if (!pre || !suc) return;
            var x1 = pre._barX2;
            var y1 = pre._barY;
            var x2 = suc._barX1;
            var y2 = suc._barY;
            var midX = (x1 + x2) / 2;
            var path = 'M ' + x1 + ' ' + y1
                + ' L ' + midX + ' ' + y1
                + ' L ' + midX + ' ' + y2
                + ' L ' + x2 + ' ' + y2;
            svg.appendChild(createSvg('path', {
                d: path,
                'class': 'gantt-arrow',
                'marker-end': 'url(#gantt-arrow)'
            }));
        });

        var today = startOfDay(new Date());
        if (today >= projectStart && today <= projectEnd) {
            var tx = labelWidth + dayDiff(projectStart, today) * dayWidth;
            svg.appendChild(createSvg('line', {
                x1: tx, y1: headerHeight, x2: tx, y2: totalHeight,
                'class': 'gantt-today'
            }));
        }

        if (container) {
            while (container.firstChild) container.removeChild(container.firstChild);
            container.appendChild(svg);
        }
        return svg;
    }

    global.PlanningGantt = { render: render };
})(window);
