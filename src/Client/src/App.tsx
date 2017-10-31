import * as React from 'react';
import Search from './Search';
import Project from './Project';

import './App.css';
import 'bootstrap/dist/css/bootstrap.css';

interface AppState {
  items: string[];
  selected?: string;
}

class App extends React.Component<{}, AppState> {

  constructor(props: {}) {
    super(props);

    this.state = {items: []};
  }

  public componentDidMount(): void {
    fetch('/r/').then(response => {
      return response.json();
    }).then(data => {
      this.setState({ items: data, selected: this.state.selected });
    });
  }

  public handleSelect(value: string): void {
    this.setState({ items: this.state.items, selected: value });
  }

  public handleUnselect(): void {
    this.setState({items: this.state.items, selected: undefined});
  }

  render() {
    if (this.state.selected) {
      return (
        <Project name={this.state.selected} onClose={() => this.handleUnselect()} />
      );
    } else {
      return (
        <div>
          <h1>Drake</h1>

          <Search
            items={this.state.items}
            onSelect={value => this.handleSelect(value)}
          />
        </div>
      );
    }
  }
}

export default App;