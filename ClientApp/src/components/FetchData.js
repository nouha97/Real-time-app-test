import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService';
import { HubConnectionBuilder } from "@microsoft/signalr";

export class FetchData extends Component {
    constructor(props) {
        super(props);
      
        this.state = {
            forecasts: [],
            loading: true,
            nick: '',
            message: '',
            messages: [],
            hubConnection: null,
        };
    }

    componentDidMount = () => {
        this.populateWeatherData();
        const nick = window.prompt('Your name:', 'John');

        const hubConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:44347/api/ChangeStatut')
            .withAutomaticReconnect()
            .build();


        this.setState({ hubConnection, nick }, () => {
            this.state.hubConnection
                .start()
                .then(() => console.log('Connection started!'))
                .catch(err => console.log('Error while establishing connection :('));

            this.state.hubConnection.on('sendToAll', (nick, receivedMessage) => {
                const text = `${nick}: ${receivedMessage}`;
                const messages = this.state.messages.concat([text]);
                this.setState({ messages });
            });
        });
    };

    sendMessage = () => {
        this.state.hubConnection
            .invoke('sendToAll', this.state.nick, this.state.message)
            .catch(err => console.error(err));

        this.setState({ message: '' });
    };
    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>User Name</th>
                        <th>Activity</th>
                        <th>Actions</th>
                       
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.id}>
                            <td>{forecast.userName}</td>
                            <td></td>
                            <td></td>
                      
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    static render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderForecastsTable(this.state.forecasts);
        FetchData.renderMessages();

        return (
            <div>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }
    static renderMessages() {
        return (
            <div>
                <br />
                <input
                    type="text"
                    value={this.state.message}
                    onChange={e => this.setState({ message: e.target.value })}
                />

                <button onClick={this.sendMessage}>Send</button>

                <div>
                    {this.state.messages.map((message, index) => (
                        <span style={{ display: 'block' }} key={index}> {message} </span>
                    ))}
                </div>
            </div>
        );
    }

    async populateWeatherData() {
        const token = await authService.getAccessToken();
        const response = await fetch('weatherforecast', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }
}
  


