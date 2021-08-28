import React from 'react';
import { Link } from 'react-router-dom';
import { useGlobalState } from '../hooks/useGlobalState';

export default function NavBar() {
    const { user, setUser } = useGlobalState();

    return (
        <nav>
            <h3>Diary App</h3>
            {
                user ?
                    <div>{user.username} <button className="btn" onClick={() => setUser(null)}>Logout</button></div> :
                    <div>
                        <Link to="/auth/login" className="btn">Login</Link>
                        <Link to="/auth/register" className="btn">Register</Link>
                    </div>
            }
        </nav>
    )
}
