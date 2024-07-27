# Timeline Generator CLI

Generate a timeline from a YAML file. Multiple export formats are supported (TimelineJS, Vis.js, Mermaid, Bootstrap). Data can be filtered.

## Usage

### Windows

```cmd
TimelineGenerator.exe sample sample.yml

TimelineGenerator.exe generate -e timelinejs sample.yml timeline
TimelineGenerator.exe generate -e visjs sample.yml timeline
TimelineGenerator.exe generate -e mermaid sample.yml timeline
TimelineGenerator.exe generate -e bootstrap sample.yml timeline
```

### Linux

```bash
TimelineGenerator.exe sample sample.yml

TimelineGenerator.exe generate -e timelinejs sample.yml timeline
TimelineGenerator.exe generate -e visjs sample.yml timeline
TimelineGenerator.exe generate -e mermaid sample.yml timeline
TimelineGenerator.exe generate -e bootstrap sample.yml timeline
```

## Installation

Download the latest version from the [releases page](https://github.com/spech66/timelinegenerator/releases).

## Todos

- [x] Define YAML format
- [x] YAML parser
- [x] CLI parser using spectre
    - sample
	- generate
- [ ] CLI filter parameters: date rang, tags, location, ...
- [ ] Add export format for [TimelineJS](https://timeline.knightlab.com/) [V3 Repo](https://github.com/NUKnightLab/TimelineJS3)
- [ ] Add export format for	[Vis.js Timeline](https://visjs.github.io/vis-timeline/) [Repo](https://github.com/visjs/vis-timeline)
- [ ] Add export format for [Mermaid in Markdown codeblock](https://mermaid.js.org/syntax/timeline.html)
- [ ] Add export format for [Bootstrap v5](https://getbootstrap.com/docs/5.3/components/card/)
