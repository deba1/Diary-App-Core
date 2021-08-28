import React from 'react';
import { Link, Redirect } from 'react-router-dom';
import { useGlobalState } from '../hooks/useGlobalState';

export default function AdminPage() {
    const { user } = useGlobalState();

    return user && user.roles.includes("Admin") ? (
        <div style={{ marginTop: "56px", padding: "15px" }}>
            <h4 className="SectionTitle">Admin Panel</h4>
            <div style={{ padding: "0 15px" }}>
                <Link to="/settings">Settings</Link><br />
                <Link to="/user-manager">User Manager</Link><br />
                <Link to="/note-manager">Notes Manager</Link>
            </div>
        </div>
    ) : <Redirect to="/auth/login" />
}
