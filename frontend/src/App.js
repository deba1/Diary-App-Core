import { BrowserRouter, Redirect, Route, Switch } from 'react-router-dom';
import './App.css';
import AddNotePage from './components/AddNotePage';
import AddUserPage from './components/AddUserPage';
import AdminPage from './components/AdminPage';
import EditNotePage from './components/EditNotePage';
import LoginPage from './components/LoginPage';
import NavBar from './components/NavBar';
import NotesManagerPage from './components/NotesManager';
import NotesPage from './components/NotesPage';
import RegisterPage from './components/RegisterPage';
import SettingsPage from './components/SettingsPage';
import TrashedNotesPage from './components/TrashedNotesPage';
import UserManagerPage from './components/UserManagerPage';
import { GlobalProvider } from './hooks/useGlobalState';

function App() {
  return (
    <GlobalProvider>
      <BrowserRouter>
        <NavBar />
        <Switch>
          <Route path="/auth/login">
            <LoginPage />
          </Route>
          <Route path="/auth/register">
            <RegisterPage />
          </Route>
          <Route exact path="/add-note">
            <AddNotePage />
          </Route>
          <Route exact path="/notes">
            <NotesPage />
          </Route>
          <Route exact path="/admin">
            <AdminPage />
          </Route>
          <Route exact path="/settings">
            <SettingsPage />
          </Route>
          <Route exact path="/user-manager">
            <UserManagerPage />
          </Route>
          <Route exact path="/note-manager">
            <NotesManagerPage />
          </Route>
          <Route exact path="/trashed-notes">
            <TrashedNotesPage />
          </Route>
          <Route exact path="/edit-note/:noteId">
            <EditNotePage />
          </Route>
          <Route exact path="/add-user">
            <AddUserPage />
          </Route>
          <Route path="*">
            <Redirect to="/auth/login" />
          </Route>
        </Switch>
      </BrowserRouter>
    </GlobalProvider>
  );
}

export default App;
