type LoginProps = {
  email: string;
  password: string;
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
