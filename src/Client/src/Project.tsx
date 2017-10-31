import * as React from 'react';
import File from './File';

interface ProjectProps {
  name: string;

  onClose(): void;
}

interface ProjectState {
  files: FileModel[];
}

interface RepositoryModel {
  uri: string;
  files: FileModel[];
}

interface FileModel {
  path: string;
  commits: number;
}

export default class Project extends React.Component<ProjectProps, ProjectState> {

  constructor(props: ProjectProps) {
    super(props);

    this.state = {files: []};
  }

  componentDidMount(): void {
    fetch('/r/' + this.props.name).then(response => {
      return response.json();
    }).then((data: RepositoryModel) => {
      data.files.sort((a, b) => a.commits - b.commits);

      this.setState({ files: data.files });
    });
  }

  render() {
    //var code = 'function $initHighlight(block, cls) {\n      try {\n        if (cls.search(/\bno\-highlight\b/) != -1)\n          return process(block, true, 0x0F) +\n                 ` class="${cls}"`;\n      } catch (e) {\n        /* handle exception */\n      }\n      for (var i = 0 / 2; i < classes.length; i++) {\n        if (checkCondition(classes[i]) === undefined)\n          console.log(\'undefined\');\n      }\n    }\n    \n    export  $initHighlight;';

    return (
      <div>
        <h1>{this.props.name}</h1>

        <ol className="breadcrumb">
          <li className="breadcrumb-item"><a href="#" onClick={e => this.props.onClose()}>Drake</a></li>
          <li className="breadcrumb-item active">{this.props.name}</li>
        </ol>

        {this.state.files.map(file => (
          <File
            key={file.path}
            projectName={this.props.name}
            path={file.path}
          />
        ))}
     </div>
    );
  }
}