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
TimelineGenerator sample sample.yml

TimelineGenerator generate -e timelinejs sample.yml timeline
TimelineGenerator generate -e visjs sample.yml timeline
TimelineGenerator generate -e mermaid sample.yml timeline
TimelineGenerator generate -e bootstrap sample.yml timeline
```

## Installation

Download the latest version from the [releases page](https://github.com/spech66/timelinegenerator/releases).

## Export formats

### TimelineJS

[TimelineJS](https://timeline.knightlab.com/) is a free, easy-to-use tool for telling stories in a timeline format.

![TimelineJS Timeline](https://media.githubusercontent.com/media/spech66/timelinegenerator/main/_Misc/timelinejs.png)

### Vis.js

[Vis.js Timeline](https://visjs.github.io/vis-timeline/examples/timeline/) is a dynamic, interactive timeline library.

![Mermaid Timeline](https://media.githubusercontent.com/media/spech66/timelinegenerator/main/_Misc/visjs.png)

### Mermaid

[Mermaid](https://mermaid.js.org/syntax/timeline.html) is a simple markdown-like script language for generating charts from text via javascript.

Exporter will generate a markdown file containing the Mermaid code. Only date and title are supported. Categories are used as sections.

![Mermaid Timeline](https://media.githubusercontent.com/media/spech66/timelinegenerator/main/_Misc/mermaid.png)

### Bootstrap

[Bootstrap v5](https://getbootstrap.com/docs/5.3/components/card/) is a free and open-source CSS framework directed at responsive, mobile-first front-end web development.

![Bootstrap Timeline](https://media.githubusercontent.com/media/spech66/timelinegenerator/main/_Misc/bootstrap.png)

## Todos

- [x] Define YAML format
- [x] YAML parser
- [x] CLI parser using spectre
    - sample
	- generate
- [x] Add export format for [TimelineJS](https://timeline.knightlab.com/) [V3 Repo](https://github.com/NUKnightLab/TimelineJS3)
- [x] Add export format for	[Vis.js Timeline](https://visjs.github.io/vis-timeline/) [Repo](https://github.com/visjs/vis-timeline)
- [x] Add export format for [Mermaid in Markdown codeblock](https://mermaid.js.org/syntax/timeline.html)
- [x] Add export format for [Bootstrap v5](https://getbootstrap.com/docs/5.3/components/card/)
- [x] CLI filter parameters: date range, tags, category
