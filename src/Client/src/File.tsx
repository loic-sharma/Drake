import * as React from 'react';
import * as hljs from 'highlight.js';
import * as ReactDOM from 'react-dom';

import 'highlight.js/styles/atom-one-light.css';

interface FileProps {
  projectName: string;
  path: string;
}

interface FileState {
  content: string;
}

export default class File extends React.Component<FileProps, FileState> {

  constructor(props: FileProps) {
    super(props);

    this.state = { content: '' };
  }

  componentDidMount() {
    fetch('/f/' + this.props.projectName + '/' + this.props.path).then(response => {
      return response.text()
    }).then(data => {
      this.setState({content: data});
    })
  }

  componentDidUpdate() {
    if (this.state.content) {
      var current = ReactDOM.findDOMNode(this);
      var code = current.getElementsByTagName('pre');
  
      hljs.highlightBlock(code[0]);
    }
  }

  render() {
    return (
      <div>
        <p>{this.props.path}</p>
        <pre>
          <code>
            {this.state.content}
          </code>
        </pre>
      </div>
    );
  }
}