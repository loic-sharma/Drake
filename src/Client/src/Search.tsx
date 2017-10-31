import * as React from 'react';

interface SearchProps {
  items: string[];

  onSelect(value: string): void;
}

interface SearchState {
  input: string;
  filteredItems: string[];
}

export default class Search extends React.Component<SearchProps, SearchState> {

  constructor(props: SearchProps) {
    super(props);

    this.state = {input: '', filteredItems: this.props.items};
  }

  public componentWillReceiveProps(props: SearchProps) {
    let items = props.items.filter(i => i.toLowerCase().indexOf(this.state.input.toLowerCase()) >= 0);
    
    this.setState({ input: this.state.input, filteredItems: items });
  }

  public handleSearch(value: string): void {
    let items = this.props.items.filter(i => i.toLowerCase().indexOf(value.toLowerCase()) >= 0);

    this.setState({ input: value, filteredItems: items });
  }

  public handleKeyDown(keyCode: number) {
    // On enter, "force" select the input. A quick sanity check is done
    // to verify this looks like a GitHub repository.
    if (keyCode === '\r'.charCodeAt(0) && this.state.input.indexOf('/') > 0) {
      this.props.onSelect(this.state.input);
    }
  }

  render() {
    return (
      <div className="Search">
        <input
          type="text"
          placeholder="Search"
          onChange={e => this.handleSearch(e.target.value)}
          onKeyDown={e => this.handleKeyDown(e.keyCode)}
        />

        <p>{this.state.filteredItems.length} results:</p>

        <ul>
          {this.state.filteredItems.map(value => (
            <li key={value}>
              <a href="#" onClick={e => this.props.onSelect(value)}>{value}</a>
            </li>
          ))}
        </ul>
      </div>
    );
  }
}