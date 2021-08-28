import axios from 'axios';
import React from 'react';
import { Link, Redirect } from 'react-router-dom';
import { rootUrl } from '../hooks/useFetcher';
import { useGlobalState } from '../hooks/useGlobalState';

const status = [
    "Disabled",
    "Enabled"
]

export default function SettingsPage() {
    const { user, features, updateFeatures } = useGlobalState();

    const onStateChange = (key) => {
        const state = features.find(f => f.id === key).status;
        axios.put(`${rootUrl}settings/${key}`, {
            status: state === 0 ? 1 : 0
        }, {
            headers: {
                Authorization: `Bearer ${user.token}`
            }
        }).then(res => {
            updateFeatures();
        });
    }

    return user ? (
        <div style={{ marginTop: "56px", padding: "15px" }}>
            <h4 style={{ marginTop: "36px" }}><Link to="/admin" className="btn">&larr;</Link>Settings</h4>
            <table width="100%" style={{ textAlign: "center" }}>
                <thead>
                    <tr>
                        <th>Setting Name</th>
                        <th>Status</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {features.map((feature, i) => (
                        <tr key={i} className="Note">
                            <td>{feature.id}</td>
                            <td>{status[feature.status]}</td>
                            <td className="Actions">
                                <button className="btn" onClick={() => onStateChange(feature.id)}>Change</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

        </div>
    ) : <Redirect to="/auth/login" />
}
