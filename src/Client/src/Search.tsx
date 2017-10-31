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

  render() {
    return (
      <div className="Search">
        <input
          type="text"
          placeholder="Search"
          onChange={e => this.handleSearch(e.target.value)}
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