type LoginProps = {
  email: string;
  password: string;
};

type SignupProps = {
  fullName: string;
  email: string;
  password: string;
};

type SignupResponse = {
  message: string;
};

export async function login({ email, password }: LoginProps): Promise<void> {
  try {
    const response = await fetch(`http://localhost:5000/account/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email, password }),
    });
    if (response.ok) {
      const { token } = await response.json();
      localStorage.setItem('jwtToken', token);
    } else {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Login failed');
    }
  } catch (e) {
    console.error(`Error occurred: ${e}`);
    throw e;
  }
}

export async function signup({ fullName, email, password }: SignupProps): Promise<SignupResponse> {
  try {
    const response = await fetch(`http://localhost:5000/account/register`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ fullName, email, password }),
    });
    if (response.ok) {
      return { message: 'Sign-up is complete' };
    } else if (response.status === 400) {
      return { message: 'Invalid data provided' };
    } else if (response.status === 500) {
      return { message: 'Server error, please try again later' };
    } else {
      return { message: 'Failed to sign-up' };
    }
  } catch (e) {
    throw new Error(`Error occurred while signing up: ${e}`);
  }
}
