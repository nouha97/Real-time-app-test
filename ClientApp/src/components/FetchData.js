import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService';
import { HubConnectionBuilder } from "@microsoft/signalr";

export class FetchData extends Component {
    constructor(props) {
        super(props);

        this.state = {
            allusers: [],
            loading: true,
            iduser : '',
            nick: '',
            message: '',
            messages: [],
            hubConnection: null,
        };
    }
    sendMessage = async () => {

        const chatMessage = {
            user: 'nuu',
            message: 'ff'
        };
        const token = await authService.getAccessToken();
        try {
            await fetch('https://localhost:44347/users/api/ChangeStatut', {
                method: 'POST',
                body: JSON.stringify({
                    ActivityS: "ON CALL",
                    ID: 'ff'
                    }),
                headers:
                    !token ? {} : {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
            }

            );
        }
        catch (e) {
            console.log('Sending message failed.', e);
        }
    }
    componentDidMount = () => {
        this.usersData();
        const nick = window.prompt('Your name:', 'John');

        const hubConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:44347/users/api/ChangeStatut')
            .withAutomaticReconnect()
            .build();


        this.setState({ hubConnection, nick }, () => {
            this.state.hubConnection
                .start()
                .then(() => console.log('Connection started!'))
                .catch(err => console.log('Error while establishing connection :('));

            this.state.hubConnection.on('ReceiveMessage', (nick, receivedMessage) => {
                console.log(nick, receivedMessage);
                const text = `${nick}: ${receivedMessage}`;
                const messages = this.state.messages.concat([text]);
                this.setState({ messages });
            });
        });
    };

    async usersData() {
        const token = await authService.getAccessToken();
        const response = await fetch('users', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();
        console.log(data);
        this.setState({ allusers: data.users, loading: false, iduser: data.userId});
    }

   
    render() {
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
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>User Name</th>
                            <th>Activity</th>
                            <th>Actions</th>

                        </tr>
                    </thead>
                    <tbody>
                        {this.state.allusers.map(user =>
                            <tr key={user.id}>
                                <td>{user.userName}</td>
                                <td></td>
                               
                                <td>
                                    {this.state.iduser == user.id ?
                                       
                                        <button onClick={this.sendMessage}>ON CALL</button>
                                        : null}
                   
                                    </td>

                            </tr>
                        )}
                    </tbody>
                </table>
            </div>

        );
    }
}
  


