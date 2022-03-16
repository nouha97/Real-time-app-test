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
           newStatut : false,
            message: '',
           
            idSocket :'',
            newActivity : '',
            hubConnection: null,
        };
    }
    sendMessage = async (activity) => {
        const token = await authService.getAccessToken();
        try {
            await fetch('https://localhost:44347/users/api/ChangeStatut', {
                method: 'POST',
                body: JSON.stringify({
                    ActivityS: activity,
                    ID: this.state.iduser
                    }),
                headers:
                    !token ? {} : {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
            }

            ).then(() => {
                this.usersData();
            });
        }
        catch (e) {
            console.log('Sending message failed.', e);
        }
    }
   

    componentDidMount = () => {
        this.usersData();
    

        const hubConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:44347/users/api/ChangeStatut')
            .withAutomaticReconnect()
            .build();


        this.setState({ hubConnection }, () => {
            this.state.hubConnection
                .start()
                .then(() => console.log('Connection started!'))
                .catch(err => console.log('Error while establishing connection :('));

            this.state.hubConnection.on('ReceiveMessage', (receivedMessage, iduser) => {
            this.setState({ newStatut: receivedMessage, newActivity: true, idSocket : iduser});
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
               
                <table className='table table-striped table-borderless' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>User name</th>
                            <th>Activity</th>
                            <th></th>

                        </tr>
                    </thead>
                    <tbody>
                        {this.state.allusers.map(user =>
                            <tr key={user.id}>
                                <td>{user.userName}</td>
                                <td>{this.state.idSocket == user.id ?
                                    <span className= "badge bg-info text-white">{this.state.newStatut}</span>
                                    : <span className= "badge bg-info text-white">{user.activity}</span>}
                                </td>
                               
                                <td>
                                    {this.state.iduser == user.id ?
                                        
                                        <button className= "btn btn-outline-success btn-sm" onClick={()=>this.sendMessage("ON CALL")}>ON CALL</button>
                                        : null}
                                    {this.state.iduser == user.id ?

                                        <button className="btn btn-outline-danger btn-sm" onClick={() => this.sendMessage("ON MEETING")}>ON MEETING</button>
                                        : null}
                                    {this.state.iduser == user.id ?

                                        <button className="btn btn-outline-secondary btn-sm" onClick={() => this.sendMessage("ON BREAK")}>ON BREAK</button>
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
  


